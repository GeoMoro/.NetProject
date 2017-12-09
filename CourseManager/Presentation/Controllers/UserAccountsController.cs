using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Presentation.Models;
using System.Collections;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Presentation.Controllers
{
    public class UserAccountsController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly IUserAccountRepository _repository;

        public UserAccountsController(IUserAccountRepository repository,IHostingEnvironment env)
        {
            _env = env;
            _repository = repository;
        }

        // GET: UserAccounts
        public IActionResult Index()
        {
            return View(_repository.GetAllUsers());
        }

        // GET: UserAccounts/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = _repository.GetUserById(id.Value);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // GET: UserAccounts/CreateStudent
        public IActionResult Createstudent()
        {
            return View();
        }

        // POST: UserAccounts/CreateStudent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudent([Bind("FirstName,LastName,RegistrationNumber,Group,Password,ConfirmPassword,Email,File")] UserAccountStudentCreateModel userAccountStudentCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userAccountStudentCreateModel);
            }

            _repository.CreateUser(
                UserAccount.CreateStudentAccount(
                    userAccountStudentCreateModel.FirstName,
                    userAccountStudentCreateModel.LastName,
                    userAccountStudentCreateModel.RegistrationNumber,
                    userAccountStudentCreateModel.Group,
                    userAccountStudentCreateModel.Password,
                    userAccountStudentCreateModel.Email
                )
            );

            var file = userAccountStudentCreateModel.File;

            if (file.Length > 0)
            {
                string path = Path.Combine(_env.WebRootPath, "Files/" + userAccountStudentCreateModel.Email);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string extension = userAccountStudentCreateModel.Email + "." + Path.GetExtension(file.FileName).Substring(1);

                using (var fileStream = new FileStream(Path.Combine(path, extension), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: UserAccounts/CreateStudent
        public IActionResult CreateAssistant()
        {
            return View();
        }

        // POST: UserAccounts/CreateAssistant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAssistant([Bind("FirstName,LastName,RegistrationNumber,Group,Password,ConfirmPassword,Email")] UserAccountAssistantCreateModel userAccountAssistantCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userAccountAssistantCreateModel);
            }

            _repository.CreateUser(
                UserAccount.CreateAssistantAccount(
                    userAccountAssistantCreateModel.FirstName,
                    userAccountAssistantCreateModel.LastName,
                    userAccountAssistantCreateModel.Password,
                    userAccountAssistantCreateModel.Email
                )
            );

            return RedirectToAction(nameof(Index));
        }

        // GET: UserAccounts/EditStudent/5
        public IActionResult EditStudent(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = _repository.GetUserById(id.Value);
            if (userAccount == null)
            {
                return NotFound();
            }

            var studentEditModel = new UserAccountStudentEditModel(
                userAccount.FirstName,
                userAccount.LastName,
                userAccount.RegistrationNumber,
                userAccount.Group,
                userAccount.Email
            );

            return View(studentEditModel);
        }

        // POST: UserAccounts/EditStudent/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStudent(Guid id, [Bind("FirstName,LastName,RegistrationNumber,Group,Email")] UserAccountStudentEditModel userAccountStudentEditModel)
        {
            var studentToBeEdited = _repository.GetUserById(id);

            if (studentToBeEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(userAccountStudentEditModel);
            }

            studentToBeEdited.FirstName = userAccountStudentEditModel.FirstName;
            studentToBeEdited.LastName = userAccountStudentEditModel.LastName;
            studentToBeEdited.RegistrationNumber = userAccountStudentEditModel.RegistrationNumber;
            studentToBeEdited.Email = userAccountStudentEditModel.Email;
            studentToBeEdited.Group = userAccountStudentEditModel.Group;

            try
            {
                _repository.EditUser(studentToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAccountExists(_repository.GetUserById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: UserAccounts/EditAdmin/5
        public IActionResult EditAdmin(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = _repository.GetUserById(id.Value);
            if (userAccount == null)
            {
                return NotFound();
            }

            var adminEditModel = new UserAccountAdminEditModel(
                userAccount.FirstName,
                userAccount.LastName,
                userAccount.Email
            );

            return View(adminEditModel);
        }

        // POST: UserAccounts/EditAdmin/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAdmin(Guid id, [Bind("FirstName,LastName,Email")] UserAccountAdminEditModel userAccountAdminEditModel)
        {
            var adminToBeEdited = _repository.GetUserById(id);

            if (id != adminToBeEdited.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(userAccountAdminEditModel);
            }

            adminToBeEdited.FirstName = userAccountAdminEditModel.FirstName;
            adminToBeEdited.LastName = userAccountAdminEditModel.LastName;
            adminToBeEdited.Email = userAccountAdminEditModel.Email;

            try
            {
                _repository.EditUser(adminToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAccountExists(adminToBeEdited.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: UserAccounts/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = _repository.GetUserById(id.Value);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var userAccount = _repository.GetUserById(id);

            _repository.DeleteUser(userAccount);

            return RedirectToAction(nameof(Index));
        }

        private bool UserAccountExists(Guid id)
        {
            return _repository.GetAllUsers().Any(e => e.Id == id);
        }

        public IActionResult FileSubmissionPage()
        {
            return View();
        }


        public IActionResult KataSubmit()
        {
            return View();
        }

        public IActionResult Downloading()
        {
            return View();
        }

        /*        public IActionResult SeminarSubmit()
                {
                    return View();
                }*/
        /* protected void UploadButton_Click(object sender, EventArgs e)
         {
             if (FileUploadControl.HasFile)
             {
                 try
                 {
                     string filename = Path.GetFileName(FileUploadControl.FileName);
                     FileUploadControl.SaveAs(Server.MapPath("~/") + filename);
                     StatusLabel.Text = "Upload status: File uploaded!";
                 }
                 catch (Exception ex)
                 {
                     StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                 }*/
        [HttpPost("UploadFiles")]
        public async void Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            
        }


        [HttpPost]
        public async void Upload(FileViewModel model)
        {
 
            var file = model.File;
            if (file.Length > 0)
            {
                string path = Path.Combine(_env.WebRootPath, "Files");

                using (var fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
                model.Source = $"/Files{file.FileName}";
                model.Extension = Path.GetExtension(file.FileName).Substring(1);
            }
        }

        [HttpPost]
        public void UploadFilesAjax()
        {
            long size = 0;
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                filename = _env.WebRootPath + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
        }

        [HttpPost]
        public IActionResult Download(Guid? id)
        {
            var userAccount = _repository.GetUserById(id.Value);
           
            string searchedPath= Directory.GetFiles(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" + userAccount.Email)[0];
            Stream file = new FileStream(searchedPath, FileMode.Open);
            string content_type = "application/octet-stream";

            return File(file, content_type, Path.GetFileName(searchedPath));
        }

        [HttpPost]
        public void Uploading(ICollection<IFormFile> files)
        {
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                         file.CopyToAsync(fileStream);
                    }
                }
            }
        }
        

    }
        }


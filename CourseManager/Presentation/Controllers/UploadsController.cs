using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Business.ServicesInterfaces.Models.UploadsViewModels;
using Data.Persistance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class UploadsController : Controller
    {
        private readonly ApplicationDbContext _application;
   
        private readonly IHostingEnvironment _env;
        public UploadsController(IHostingEnvironment env, ApplicationDbContext application)
        {
            _env = env;
            _application = application;
        }
        // GET: Uploads
        public IActionResult Index()
        {
            //IIdentity LoggedInUser = User.Identity;
            return View();
        }

// GET: Uploads/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: Uploads/Create
        public IActionResult Create(Guid userId)
        {
            return View();
        }

        // POST: Uploads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string userGroup, string userFirstName, string userLastName, [Bind("Type,Seminar,File")] UploadsCreateModel uploadCreateModel)
        {
            //long size = uploadCreateModel.File;

            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            var file = uploadCreateModel.File;
            if (file.Length > 0)
            {
                string path = Path.Combine(_env.WebRootPath,
                    "Uploads/" + uploadCreateModel.Type + "//" + uploadCreateModel.Seminar);
                var extensions = new List<string>
                {
                    ".zip",
                    ".rar",
                    ".7z"
                };
                var types = new List<string>
                {
                    "Seminar",
                    "Laboratory",
                    "Kata"
                };
                if (!extensions.Contains(Path.GetExtension(file.FileName)) || !types.Contains(uploadCreateModel.Type))
                    return View("Create");
                
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    string extension = userFirstName + "" + userLastName + "" + userGroup + "." +
                                       Path.GetExtension(file.FileName).Substring(1);
                    using (var fileStream = new FileStream(Path.Combine(path, extension), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                
            }


            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return RedirectToAction(nameof(Index));
            
        }

        // GET: Uploads/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: Uploads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Uploads/Delete/5
        public IActionResult Delete(Guid? id)
        {
            return View();
        }

        // POST: Uploads/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Download(string seminarName, string group, string seminarNumber, string fileName)
        {
            {
                string searchedPath = Path.Combine(_env.WebRootPath, "Uploads/" + seminarName + "/" + seminarNumber + "/" + fileName);

                Stream file = new FileStream(searchedPath, FileMode.Open);
                string content_type = "application/octet-stream";

                return File(file, content_type, fileName);
            }
        }

    }
}
using System;
using System.IO;
using System.Threading.Tasks;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.UploadsViewModels;
using Data.Persistance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class UploadsController : Controller
    {
        private readonly IUploadService _service;
        private readonly IHostingEnvironment _env;

        public UploadsController(IUploadService service)
        {
            _service = service;
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
            await _service.CreateUploads(userGroup, userFirstName, userLastName, uploadCreateModel);
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
                var file = _service.DownloadFile(seminarName, group, seminarNumber, fileName);

                return File(file, "application/octet-stream", fileName);
            }
        }

    }
}
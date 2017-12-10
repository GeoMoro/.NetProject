using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Presentation.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace Presentation.Controllers
{
    public class KatasController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly IKataRepository _repository;

        public KatasController(IKataRepository repository, IHostingEnvironment env)
        {
            _env = env;
            _repository = repository;
        }

        // GET: Katas
        public IActionResult Index()
        {
            return View(_repository.GetAllKatas());
        }

        // GET: Katas/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kata = _repository.GetKataById(id.Value);
            if (kata == null)
            {
                return NotFound();
            }


            return View(kata);
        }

        // GET: Katas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Katas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,File")] KataCreateModel kataCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(kataCreateModel);
            }

            _repository.CreateKata(
                Kata.CreateKata(
                    kataCreateModel.Title,
                    kataCreateModel.Description
                )
            );
            foreach (var file in kataCreateModel.File)
            {
                if (file.Length > 0)
                {
                    string path = Path.Combine(_env.WebRootPath, "Katas/" + kataCreateModel.Title);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    
                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Katas/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kata = _repository.GetKataById(id.Value);
            if (kata == null)
            {
                return NotFound();
            }

            var kataEditModel = new KataEditModel(
                kata.Title,
                kata.Description
            );

            return View(kataEditModel);
        }

        // POST: Katas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description,File")] KataEditModel kataModel)
        {
            var kataEdited = _repository.GetKataById(id);

            if (kataEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(kataModel);
            }

            var oldTitle = kataEdited.Title;

            kataEdited.Title = kataModel.Title;
            kataEdited.Description = kataModel.Description;

            try
            {
                _repository.EditKata(kataEdited);

                string searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + oldTitle);

                if (Directory.Exists(searchedPath))
                {
                    Directory.Delete(searchedPath, true);
                }

                foreach (var file in kataModel.File)
                {
                    if (file.Length > 0)
                    {
                        string path = Path.Combine(_env.WebRootPath, "Lectures/" + kataModel.Title);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        
                        using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KataExists(_repository.GetKataById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Katas/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kata = _repository.GetKataById(id.Value);

            if (kata == null)
            {
                return NotFound();
            }

            return View(kata);
        }

        // POST: Katas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var kata = _repository.GetKataById(id);

            string searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + kata.Title);
            Directory.Delete(searchedPath, true);

            _repository.DeleteKata(kata);

            return RedirectToAction(nameof(Index));
        }

        private bool KataExists(Guid id)
        {
            return _repository.GetAllKatas().Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult DeleteFile(string title, string fileName, Guid? givenId)
        {
            {
                string searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + title + "/" + fileName);
                if ((System.IO.File.Exists(searchedPath)))
                {
                    System.IO.File.Delete(searchedPath);
                }
            }

            return RedirectToAction("Delete", "Katas", new { id = givenId });
        }

        [HttpPost]
        public IActionResult Download(string title, string fileName)
        {
            {
                string searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + title + "/" + fileName);
                Stream file = new FileStream(searchedPath, FileMode.Open);
                string content_type = "application/octet-stream";

                return File(file, content_type, fileName);
            }
        }
    }
}

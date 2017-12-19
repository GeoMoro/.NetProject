using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models.KataViewModels;

namespace Presentation.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Katas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Assistant")]
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

            var currentKata = _repository.GetKataInfoByDetails(kataCreateModel.Title, kataCreateModel.Description);

            if (kataCreateModel.File != null)
            {
                foreach (var file in kataCreateModel.File)
                {
                    if (file.Length > 0)
                    {
                        string path = Path.Combine(_env.WebRootPath, "Katas/" + currentKata.Id);

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
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Katas/Edit/5
        [Authorize(Roles = "Owner, Assistant")]
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
        [Authorize(Roles = "Owner, Assistant")]
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

                //string searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + oldTitle);

                //if (Directory.Exists(searchedPath))
                //{
                //    Directory.Delete(searchedPath, true);
                //}

                if (kataModel.File != null)
                {

                    foreach (var file in kataModel.File)
                    {
                        if (file.Length > 0)
                        {
                            string path = Path.Combine(_env.WebRootPath, "Katas/" + kataEdited.Id);

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
        [Authorize(Roles = "Owner, Assistant")]
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
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var kata = _repository.GetKataById(id);

            string searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + kata.Id);
            if (Directory.Exists(searchedPath))
            {
                Directory.Delete(searchedPath, true);
            }

            _repository.DeleteKata(kata);

            return RedirectToAction(nameof(Index));
        }

        private bool KataExists(Guid id)
        {
            return _repository.GetAllKatas().Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult DeleteFile(string fileName, Guid? givenId)
        {
            {
                string searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + givenId.Value + "/" + fileName);
                if ((System.IO.File.Exists(searchedPath)))
                {
                    System.IO.File.Delete(searchedPath);
                }
            }

            return RedirectToAction("Delete", "Katas", new { id = givenId });
        }

        [HttpPost]
        public IActionResult Download(Guid kataId, string fileName)
        {
            {
                string searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + kataId + "/" + fileName);
                Stream file = new FileStream(searchedPath, FileMode.Open);
                string content_type = "application/octet-stream";

                return File(file, content_type, fileName);
            }
        }
    }
}

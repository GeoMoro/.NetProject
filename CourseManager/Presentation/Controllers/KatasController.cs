using System;
using System.Threading.Tasks;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.KataViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Authorize]
    public class KatasController : Controller
    {
        private readonly IKataService _service;

        public KatasController(IKataService service)
        {
            _service = service;
        }

        // GET: Katas
        public IActionResult Index()
        {
            return View(_service.GetAllKatas());
        }

        // GET: Katas/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kata = _service.GetKataById(id.Value);
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

            await _service.CreateKata(kataCreateModel);

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

            var kata = _service.GetKataById(id.Value);
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
            var kataEdited = _service.GetKataById(id);

            if (kataEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(kataModel);
            }
            
            kataEdited.Title = kataModel.Title;
            kataEdited.Description = kataModel.Description;

            try
            {
                await _service.Edit(id, kataEdited, kataModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KataExists(_service.GetKataById(id).Id))
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

            var kata = _service.GetKataById(id.Value);

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
            var kata = _service.GetKataById(id);

            _service.DeleFromPath(id);

            _service.DeleteKata(kata);
            return RedirectToAction(nameof(Index));
        }

        private bool KataExists(Guid id)
        {
            return _service.GetAll(id);
        }

        [HttpPost]
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult DeleteFile(string fileName, Guid? givenId)
        {
            _service.DeleteFile(fileName, givenId);
            return RedirectToAction("Delete", "Katas", new { id = givenId });
        }

        [HttpPost]
        public IActionResult Download(Guid kataId, string fileName)
        {
            var file = _service.SearchKata(kataId, fileName);

            return File(file, "application/octet-stream", fileName);
        }
    }
}

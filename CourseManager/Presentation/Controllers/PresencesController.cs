using System;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models.PresenceViewModels;

namespace Presentation.Controllers
{
    public class PresencesController : Controller
    {
        private readonly IPresenceRepository _repository;

        public PresencesController(IPresenceRepository repository)
        {
            _repository = repository;
        }

        //GET: Presences
        public IActionResult Index()
        {
            return View(_repository.GetAllPresences());
        }

        public IActionResult CreatePresence()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePresence([Bind("Laboratory, Present")] PresenceCreateModel presenceCreateModel) {
            if (!ModelState.IsValid) {
                return View(presenceCreateModel);
            }

            _repository.CreatePresence(
                Presence.CreatePresence(
                    presenceCreateModel.Laboratory,
                    presenceCreateModel.Present
                )
            );

            return RedirectToAction(nameof(Index));
        }

        // GET: UserAccounts/EditStudent/5
        public IActionResult UpdatePresence(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var presence = _repository.GetPresenceById(id.Value);
            if (presence == null) {
                return NotFound();
            }

            var presenceEditModel = new PresenceEditModel(
                presence.Present
            );

            return View(presenceEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePresence(Guid id, [Bind("Laboratory, Present")] PresenceEditModel presenceEditModel) {
            var presenceToBeEdited = _repository.GetPresenceById(id);

            if (presenceToBeEdited == null) {
                return NotFound();
            }

            if (!ModelState.IsValid) {
                return View(presenceEditModel);
            }

            presenceToBeEdited.Present = presenceEditModel.Present;

            try {
                _repository.UpdatePresence(presenceToBeEdited);
            } catch (DbUpdateConcurrencyException) {
                if (!PresenceExists(_repository.GetPresenceById(id).Id)) {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: UserAccounts/Delete/5
        public IActionResult Delete(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var presence = _repository.GetPresenceById(id.Value);
            if (presence == null) {
                return NotFound();
            }

            return View(presence);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id) {
            var presence = _repository.GetPresenceById(id);

            _repository.DeletePresence(presence);

            return RedirectToAction(nameof(Index));
        }

        private bool PresenceExists(Guid id) {
            return _repository.GetAllPresences().Any(e => e.Id == id);
        }
    }
}
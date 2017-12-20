using System;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using Presentation.Models.PresenceViewModels;
using Presentation.Data;
using System.Collections.Generic;

namespace Presentation.Controllers
{
    public class PresencesController : Controller
    {
        private readonly IPresenceRepository _repository;
        private readonly ApplicationDbContext _application;
        private readonly IUserStatusRepository _userRepo;

        public PresencesController(IPresenceRepository repository, ApplicationDbContext application, IUserStatusRepository userRepo)
        {
            _repository = repository;
            _application = application;
            _userRepo = userRepo;
        }

        //GET: Presences
        public IActionResult Index()
        {
            foreach(var item in _repository.GetAllPresences())
            {
                item.Students = _userRepo.GetUsersByLaboratory(item.Id).ToList();
            }

            return View(_repository.GetAllPresences());
        }

        public IActionResult CreateLaboratory()
        {
            return View();
        }

        public IActionResult Attendance()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateLaboratory([Bind("Name")] PresenceCreateModel presenceCreateModel)
        {
            if (!ModelState.IsValid) {
                return View(presenceCreateModel);
            }
            
            _repository.CreatePresence(
                Presence.CreatePresence(
                    presenceCreateModel.Name
                )
            );
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Attendance(string userId, [Bind("Presence")] UserStatusCreateModel userCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userCreateModel);
            }

            var laboratory = _repository.GetAllPresences().LastOrDefault();

            if (laboratory != null)
                _userRepo.CreateUser(
                    UserStatus.CreateUsersStatus(
                        userId,
                        laboratory.Id,
                        0,
                        0,
                        userCreateModel.Presence
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

            //var presenceEditModel = new PresenceEditModel(
            //    presence.Name,
            //    presence.Week
            //);

            return View("Index");
        }
        
        public IActionResult UpdateAttendance(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userStatus = _userRepo.GetUserById(id);
            if (userStatus == null)
            {
                return NotFound();
            }

            var userStatusEditModel = new UserStatusEditModel(
                userStatus.Presence,
                userStatus.LaboratoryMark,
                userStatus.KataMark
            );

            return View(userStatusEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAttendance(string id, [Bind("LaboratoryMark, KataMark, Presence")] UserStatusEditModel userStatusEditModel)
        {
            var userStatusToBeEdited = _userRepo.GetUserById(id);

            if (userStatusToBeEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(userStatusEditModel);
            }

            userStatusToBeEdited.Presence = userStatusEditModel.Presence;
            userStatusToBeEdited.LaboratoryMark = userStatusEditModel.LaboratoryMark;
            userStatusToBeEdited.KataMark = userStatusEditModel.KataMark;

            try
            {
                _userRepo.EditUser(userStatusToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserStatusExists(_userRepo.GetUserById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
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

            presenceToBeEdited.Name = presenceEditModel.Name;

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

        private bool UserStatusExists(string id)
        {
            return _userRepo.GetAllUsers().Any(e => e.Id == id);
        }
    }
}
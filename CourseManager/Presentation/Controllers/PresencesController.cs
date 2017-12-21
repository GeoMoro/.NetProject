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
using System.Diagnostics;
using Data.Domain.Interfaces.ServicesInterfaces;

namespace Presentation.Controllers
{
    public class PresencesController : Controller
    {
        private readonly IPresenceRepository _repository;
        private readonly ApplicationDbContext _application;
        private readonly IUserStatusRepository _userRepo;
        private readonly IUserStatusService _service;

        public PresencesController(IPresenceRepository repository, ApplicationDbContext application, IUserStatusRepository userRepo, IUserStatusService service)
        {
            _repository = repository;
            _application = application;
            _userRepo = userRepo;
            _service = service;
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateLaboratory([Bind("Name")] PresenceCreateModel presenceCreateModel)
        {
            if (!ModelState.IsValid) {
                return View(presenceCreateModel);
            }
            
            var selectedStudents = new List<UserStatus>();
            Guid randomLab = new Guid();
            
            foreach (var student in _application.Users.ToList())
            {
                if (presenceCreateModel.Name.Equals(student.Group))
                {
                    selectedStudents.Add(
                            _service.CreateAndReturnLatestUser(student.Id, randomLab, 0, 0, false)
                    );
                }
            }
            
            _repository.CreatePresence(
                Presence.CreatePresence(
                    presenceCreateModel.Name,
                    selectedStudents
                )
            );

            var modify = _repository.GetAllPresences().OrderBy(user => user.StartDate).LastOrDefault()?.Id;
            var searchedUser = new UserStatus();
            foreach (var student in selectedStudents)
            {
                if (modify != null)
                {
                    try
                    {
                        searchedUser = _userRepo.GetUserById(student.Id);
                        searchedUser.LaboratoryId = modify.Value;
                        _userRepo.EditUser(searchedUser);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserStatusExists(_userRepo.GetUserById(student.Id).Id))
                        {
                            return NotFound();
                        }

                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Attendance()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Attendance(string userId, [Bind("Presence")] UserStatusCreateModel userCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userCreateModel);
            }

            foreach (var student in _application.Users.ToList())
            {
                if (student.Id == userId)
                {
                    try
                    {
                        var searchedUser = _userRepo.GetUserById(student.Id);
                        searchedUser.Presence = userCreateModel.Presence;
                        _userRepo.EditUser(searchedUser);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserStatusExists(_userRepo.GetUserById(student.Id).Id))
                        {
                            return NotFound();
                        }

                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        /*
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
                presence.Name
            );

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePresence(Guid id, [Bind("Name")] PresenceEditModel presenceEditModel)
        {
            var presenceToBeEdited = _repository.GetPresenceById(id);

            if (presenceToBeEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(presenceEditModel);
            }

            presenceToBeEdited.Name = presenceEditModel.Name;

            try
            {
                _repository.UpdatePresence(presenceToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PresenceExists(_repository.GetPresenceById(id).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }*/

        public IActionResult UpdatePresence(string id)
        {
            TempData["value"] = id;
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
        public IActionResult UpdatePresence(string id, [Bind("LaboratoryMark, KataMark, Presence")] UserStatusEditModel userStatusEditModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userStatusEditModel);
            }

            foreach (var student in _application.Users.ToList())
            {
                if (student.Id == id)
                {
                    try
                    {
                        var searchedUser = _userRepo.GetUserById(student.Id);
                        searchedUser.Presence = userStatusEditModel.Presence;
                        searchedUser.KataMark = userStatusEditModel.KataMark;
                        searchedUser.LaboratoryMark = userStatusEditModel.LaboratoryMark;

                        _userRepo.EditUser(searchedUser);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserStatusExists(_userRepo.GetUserById(student.Id).Id))
                        {
                            return NotFound();
                        }

                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        
        /*
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
        */
        private bool PresenceExists(Guid id) {
            return _repository.GetAllPresences().Any(e => e.Id == id);
        }

        private bool UserStatusExists(string id)
        {
            return _userRepo.GetAllUsers().Any(e => e.Id == id);
        }
    }
}
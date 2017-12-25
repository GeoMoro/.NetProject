using System;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models.PresenceViewModels;
using Presentation.Data;
using System.Collections.Generic;
using Data.Domain.Interfaces.ServicesInterfaces;

namespace Presentation.Controllers
{
    public class PresencesController : Controller
    {
        private readonly IPresenceRepository _repository;
        private readonly ApplicationDbContext _application;
        private readonly IUserStatusRepository _userRepo;
        private readonly IUserStatusService _service;
        private readonly IAttendanceRepository _attendance;

        public PresencesController(IPresenceRepository repository, ApplicationDbContext application, IUserStatusRepository userRepo, IUserStatusService service, IAttendanceRepository attendance)
        {
            _repository = repository;
            _application = application;
            _userRepo = userRepo;
            _service = service;
            _attendance = attendance;
        }

        //GET: Presences
        public IActionResult Index()
        {
            foreach(var item in _repository.GetAllPresences())
            {
                item.Students = _service.GetUsersByFactionId(item.Id).ToList();
                foreach(var student in item.Students)
                {
                    student.Attendance = _attendance.GetAttendanceById(student.Id);
                }
            }

            return View(_repository.GetAllPresences());
        }

        public IActionResult CreateFaction()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateFaction([Bind("Name")] PresenceCreateModel presenceCreateModel)
        {
            if (!ModelState.IsValid) {
                return View(presenceCreateModel);
            }

            var check = _repository.GetAllPresences().Where(presences => presences.Name.Contains(presenceCreateModel.Name)).ToList();

            if (check.Count == 0)
            {
                var selectedStudents = new List<UserStatus>();

                foreach (var student in _application.Users.ToList())
                {
                    if (student.Group != null && presenceCreateModel.Name.Contains(student.Group))
                    {
                        selectedStudents.Add(
                                _service.CreateAndReturnLatestUser(student.Id)
                        );
                    }
                }

                _repository.CreatePresence(
                    Presence.CreatePresence(
                        presenceCreateModel.Name,
                        selectedStudents
                    )
                );

                var modify = _repository.GetPresenceByName(presenceCreateModel.Name);
                var searchedUser = new UserStatus();
                foreach (var student in selectedStudents)
                {
                    try
                    {
                        searchedUser = _userRepo.GetUserById(student.Id);
                        searchedUser.FactionId = modify.Id;
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

        public IActionResult StartLaboratory(Guid factionId, int labValue)
        {
            var laboratory = Guid.NewGuid();
            var studentList = _service.GetUsersByFactionId(factionId);
            var getUser = _userRepo.GetAllUsers().Where(user => user.FactionId == factionId).FirstOrDefault();
            var check = _attendance.GetAllAttendances().Where(attend => attend.UserId == getUser.Id && attend.LaboratoryNumber == labValue).ToList().Count;

            if (check == 0)
            {
                foreach (var students in studentList)
                {
                    _attendance.CreateAttendance(
                        Attendance.CreateAttendance(
                            labValue,
                            laboratory,
                            students.Id,
                            0,
                            0,
                            false
                        ));
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult MarkAsPresent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsPresent(string userId, [Bind("Presence")] UserStatusCreateModel userCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userCreateModel);
            }

            try
            {
                var check = _attendance.GetAllAttendances().Where(attend => attend.UserId == userId).OrderByDescending(attend => attend.StartDate).FirstOrDefault();
                check.Presence = userCreateModel.Presence;

                _attendance.EditAttendance(check);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserStatusExists(_userRepo.GetUserById(userId).Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdatePresence(Guid attendanceId)
        {
            TempData["value"] = attendanceId;

            if (attendanceId == null)
            {
                return NotFound();
            }

            var userActivity = _attendance.GetAllAttendances().SingleOrDefault(user => user.Id == attendanceId);
            if (userActivity == null)
            {
                return NotFound();
            }

            var userEditActivity = new UserStatusEditModel(
                userActivity.Presence,
                userActivity.LaboratoryMark,
                userActivity.KataMark
            );

            return View(userEditActivity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePresence(Guid attendanceId, [Bind("LaboratoryMark, KataMark, Presence")] UserStatusEditModel userStatusEditModel)
        {
            var userToBeEdited = _attendance.GetAllAttendances().SingleOrDefault(user => user.Id == attendanceId);

            if (userToBeEdited == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(userStatusEditModel);
            }

            userToBeEdited.Presence = userStatusEditModel.Presence;
            userToBeEdited.LaboratoryMark = userStatusEditModel.LaboratoryMark;
            userToBeEdited.KataMark = userStatusEditModel.KataMark;

            try
            {
                _attendance.EditAttendance(userToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserStatusExists(_userRepo.GetUserById(userToBeEdited.UserId).Id))
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
            if (id == null) {
                return NotFound();
            }

            var presence = _repository.GetPresenceById(id.Value);

            if (presence == null)
            {
                return NotFound();
            }

            var userStatus = _userRepo.GetAllUsers().Where(user => user.FactionId == id.Value);
            
            if(userStatus == null)
            {
                return NotFound();
            }

            return View(presence);
        }
        
        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var presence = _repository.GetPresenceById(id);
            var studentList = _userRepo.GetAllUsers().Where(user => user.FactionId == id);

            foreach (var student in studentList)
            {
                var attendance = _attendance.GetAllAttendances().Where(attend => attend.UserId == student.Id).ToList();

                foreach (var attend in attendance)
                {
                    _attendance.DeleteAttendance(attend);
                }

                _userRepo.DeleteUser(student);
            }
            
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
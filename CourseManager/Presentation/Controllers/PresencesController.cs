using System;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.PresenceViewModels;
using Data.Persistance;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Authorize]
    public class PresencesController : Controller
    {
        private readonly IPresenceRepository _repository;
        private readonly IUserStatusRepository _userRepo;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IUserStatusService _service;
        private readonly IPresenceService _presenceServ;
        private readonly IUserAttendanceService _attServ;

        public PresencesController(IPresenceRepository repository, ApplicationDbContext application, IUserStatusRepository userRepo, IUserStatusService service, IAttendanceRepository attendanceRepository, IPresenceService presenceServ, IUserAttendanceService attServ)
        {
            _repository = repository;
            _userRepo = userRepo;
            _service = service;
            _attendanceRepository = attendanceRepository;
            _presenceServ = presenceServ;
            _attServ = attServ;
        }

        //GET: Presences
        public IActionResult Index()
        {
            foreach(var item in _repository.GetAllPresences())
            {
                item.Students = _service.GetUsersByFactionId(item.Id).ToList();
                foreach(var student in item.Students)
                {
                    student.Attendance = _attServ.GetAttendanceByUserId(student.Id).OrderBy(att => att.StartDate).ToList();
                }
            }

            return View(_repository.GetAllPresences());
        }

        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult CreateFaction()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult CreateFaction([Bind("Name")] PresenceCreateModel presenceCreateModel)
        {
            if (!ModelState.IsValid) {
                return View(presenceCreateModel);
            }

            var factionId = Guid.NewGuid();

            _repository.CreatePresence(
                    Presence.CreatePresence(
                        factionId,
                        presenceCreateModel.Name,
                        _presenceServ.GetUsersGivenGroup(presenceCreateModel.Name, factionId)
                    )
                );
                
               // _presenceServ.ApplyModificationsOnUsers(presenceCreateModel.Name, selectedStudents);
            
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult StartLaboratory(Guid factionId, int labValue)
        {
            _presenceServ.StartLaboratoryBasedOnValue(factionId, labValue);
            
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

            var check = _attendanceRepository.GetAllAttendances().Where(att => att.UserId == userId).ToList().Count;

            if (check != 0)
            {
                try
                {
                    var modifyPresence = _attendanceRepository.GetAllAttendances().Where(attend => attend.UserId == userId).OrderByDescending(attend => attend.StartDate).FirstOrDefault();
                    if (modifyPresence != null)
                    {
                        modifyPresence.Presence = userCreateModel.Presence;

                        _attendanceRepository.EditAttendance(modifyPresence);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserStatusExists(_userRepo.GetUserById(userId).Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult UpdatePresence(Guid attendanceId)
        {
            TempData["value"] = attendanceId;

            if (attendanceId.Equals(null))
            {
                return NotFound();
            }

            var userActivity = _attendanceRepository.GetAllAttendances().SingleOrDefault(user => user.Id == attendanceId);
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
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult UpdatePresence(Guid attendanceId, [Bind("LaboratoryMark, KataMark, Presence")] UserStatusEditModel userStatusEditModel)
        {
            var userToBeEdited = _attendanceRepository.GetAllAttendances().SingleOrDefault(user => user.Id == attendanceId);

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
                _attendanceRepository.EditAttendance(userToBeEdited);
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
        [Authorize(Roles = "Owner, Assistant")]
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
            
            if(userStatus.Equals(null))
            {
                return NotFound();
            }

            return View(presence);
        }
        
        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _attServ.DeleteData(id);
            return RedirectToAction(nameof(Index));
        }
        
        private bool UserStatusExists(string id)
        {
            return _userRepo.GetAllUsers().Any(e => e.Id == id);
        }
    }
}
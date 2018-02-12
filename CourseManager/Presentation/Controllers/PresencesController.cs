using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.PresenceViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Authorize]
    public class PresencesController : Controller
    {
        private readonly IPresenceService _service;

        public PresencesController(IPresenceService service)
        {
            _service = service;
        }

        //GET: Presences
        public IActionResult Index()
        {
            foreach(var item in _service.GetAllPresences())
            {
                item.Students = _service.GetUsersByFactionId(item.Id).ToList();
                foreach(var student in item.Students)
                {
                    student.Attendance = _service.GetAttendanceByUserId(student.Id).OrderBy(att => att.StartDate).ToList();
                }
            }

            return View(_service.GetAllPresences());
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

            _service.CreateFaction(presenceCreateModel);
            
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult StartPresence(Guid factionId, int labValue)
        {
            _service.StartPresenceBasedOnValue(factionId, labValue);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Owner, Assistant")]
        public IActionResult StopPresence(Guid factionId)
        {
            _service.StopPresence(factionId);

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

            var check = _service.GetAllAttendances().Where(att => att.UserId == userId).ToList().Count;

            if (check != 0)
            {
                try
                {
                    _service.ModifyPresence(userId, userCreateModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.UserStatusExists(_service.GetUserById(userId).Id))
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

            var userActivity = _service.GetAllAttendances().SingleOrDefault(user => user.Id == attendanceId);
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
            var userToBeEdited = _service.GetAllAttendances().SingleOrDefault(user => user.Id == attendanceId);

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
                _service.EditAttendance(userToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_service.UserStatusExists(_service.GetUserById(userToBeEdited.UserId).Id))
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

            var presence = _service.GetPresenceById(id.Value);

            if (presence == null)
            {
                return NotFound();
            }

            var userStatus = _service.GetAllUsers().Where(user => user.FactionId == id.Value);
            
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
            _service.DeleteData(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
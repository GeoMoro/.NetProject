using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServicesProvider
{
    public class PresenceService : IPresenceService
    {
        private readonly IPresenceRepository _repository;
        private readonly IUserStatusRepository _userRepo;
        private readonly IUserStatusService _service;
        private readonly IAttendanceRepository _attendance;

        public PresenceService(IPresenceRepository repository, IUserStatusRepository userRepo, IUserStatusService service, IAttendanceRepository attendance)
        {
            _repository = repository;
            _userRepo = userRepo;
            _service = service;
            _attendance = attendance;
        }

        public void StartLaboratoryBasedOnValue(Guid factionId, int labValue)
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
        }

        public void ApplyModificationsOnUsers(string name, List<UserStatus> selectedStudents)
        {
            var modify = _repository.GetPresenceByName(name);
            var searchedUser = new UserStatus();
            foreach (var student in selectedStudents)
            {
                searchedUser = _userRepo.GetUserById(student.Id);
                searchedUser.FactionId = modify.Id;
                _userRepo.EditUser(searchedUser);
            }
        }
    }
}

using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Persistance;

namespace ServicesProvider
{
    public class PresenceService : IPresenceService
    {
        private readonly IPresenceRepository _repository;
        private readonly IUserStatusRepository _userRepo;
        private readonly IUserStatusService _service;
        private readonly IAttendanceRepository _attendance;
        private readonly ApplicationDbContext _application;

        public PresenceService(IPresenceRepository repository, ApplicationDbContext application,
            IUserStatusRepository userRepo, IUserStatusService service, IAttendanceRepository attendance)
        {
            _repository = repository;
            _userRepo = userRepo;
            _service = service;
            _attendance = attendance;
            _application = application;
        }

        public void StartLaboratoryBasedOnValue(Guid factionId, int labValue)
        {
            var laboratory = Guid.NewGuid();
            var studentList = _service.GetUsersByFactionId(factionId);
            var getUser = _userRepo.GetAllUsers().Where(user => user.FactionId == factionId).FirstOrDefault();
            var check = _attendance.GetAllAttendances()
                .Where(attend => attend.UserId == getUser.Id && attend.LaboratoryNumber == labValue).ToList().Count;

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
            foreach (var student in selectedStudents)
            {
                var searchedUser = _userRepo.GetUserById(student.Id);
                searchedUser.FactionId = modify.Id;
                _userRepo.EditUser(searchedUser);
            }
        }

        public List<UserStatus> GetUsersGivenGroup(string name, Guid factionId)
        {
            var check = _repository.GetAllPresences().Where(presences => presences.Name.Contains(name)).ToList();
            
            var selectedStudents = new List<UserStatus>();

            if (check.Count == 0)
            {
                foreach (var student in _application.Users.ToList())
                {
                    if (student.Group != null && name.Contains(student.Group))
                    {
                        selectedStudents.Add(
                            _service.CreateAndReturnLatestUser(student.Id, factionId)
                        );
                    }
                }
            }

            return selectedStudents;
        }
    }
}

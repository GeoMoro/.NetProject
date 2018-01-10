using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.ServicesInterfaces;

namespace ServicesProvider
{
    public class UserAttendanceService : IUserAttendanceService
    {
        private readonly IPresenceRepository _repository;
        private readonly DatabaseContext _databaseService;
        private readonly IUserStatusRepository _userRepo;
        private readonly IUserStatusService _service;
        private readonly IAttendanceRepository _attendance;
        private readonly IPresenceService _presenceServ;

        public UserAttendanceService(IPresenceRepository repository, DatabaseContext databaseService, IUserStatusRepository userRepo, IUserStatusService service, IAttendanceRepository attendance, IPresenceService presenceServ)
        {
            _repository = repository;
            _databaseService = databaseService;
            _userRepo = userRepo;
            _service = service;
            _attendance = attendance;
            _presenceServ = presenceServ;
        }

        public List<Attendance> GetAttendanceByUserId(string id)
        {
            return _databaseService.Attendances.Where(attendance => attendance.UserId == id).ToList();
        }

        public void DeleteData(Guid id)
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
        }
    }
}

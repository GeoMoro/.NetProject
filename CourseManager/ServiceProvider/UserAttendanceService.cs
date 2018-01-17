using Data.Domain.Entities;
using Data.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.ServicesInterfaces;

namespace ServicesProvider
{
    public class UserAttendanceService : IUserAttendanceService
    {
        private readonly IPresenceRepository _repository;
        private readonly IUserStatusRepository _userRepo;
        private readonly IAttendanceRepository _attendance;

        public UserAttendanceService(IPresenceRepository repository, IUserStatusRepository userRepo, IAttendanceRepository attendance)
        {
            _repository = repository;
            _userRepo = userRepo;
            _attendance = attendance;
        }

        public List<Attendance> GetAttendanceByUserId(string id)
        {
            return _attendance.GetAllAttendances().Where(attendance => attendance.UserId == id).ToList();
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

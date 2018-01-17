using Data.Domain.Entities;
using Data.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.PresenceViewModels;
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
        private readonly IUserAttendanceService _serviceAttendance;

        public PresenceService(IPresenceRepository repository, ApplicationDbContext application, IUserStatusRepository userRepo, IUserStatusService service, IAttendanceRepository attendance, IUserAttendanceService serviceAttendance)
        {
            _repository = repository;
            _userRepo = userRepo;
            _service = service;
            _attendance = attendance;
            _application = application;
            _serviceAttendance = serviceAttendance;
        }

        //Presence Repo
        
        public IReadOnlyList<Presence> GetAllPresences()
        {
            return _repository.GetAllPresences();
        }

        public Presence GetPresenceById(Guid id)
        {
            return _repository.GetPresenceById(id);
        }
        
        //UserStatus service

        public IReadOnlyList<UserStatus> GetUsersByFactionId(Guid id)
        {
            return _service.GetUsersByFactionId(id);
        }

        //UserStatus repo

        public IReadOnlyList<UserStatus> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public UserStatus GetUserById(string id)
        {
            return _userRepo.GetUserById(id);
        }
        
        //Attendance repo

        public IReadOnlyList<Attendance> GetAllAttendances()
        {
            return _attendance.GetAllAttendances();
        }
        
        public void EditAttendance(Attendance attendance)
        {
            _attendance.EditAttendance(attendance);
        }
        
        public void StartLaboratoryBasedOnValue(Guid factionId, int labValue)
        {
            var laboratory = Guid.NewGuid();
            var studentList = _service.GetUsersByFactionId(factionId);
            var getUser = _userRepo.GetAllUsers().FirstOrDefault(user => user.FactionId == factionId);
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

        public void CreateFaction(PresenceCreateModel presenceCreateModel)
        {
            var factionId = Guid.NewGuid();

            _repository.CreatePresence(
                Presence.CreatePresence(
                    factionId,
                    presenceCreateModel.Name,
                    GetUsersGivenGroup(presenceCreateModel.Name, factionId)
                )
            );
        }

        public bool UserStatusExists(string id)
        {
            return _userRepo.GetAllUsers().Any(e => e.Id == id);
        }

        public void ModifyPresence(string userId, UserStatusCreateModel userCreateModel)
        {
            var modifyPresence = GetAllAttendances().Where(attend => attend.UserId == userId).OrderByDescending(attend => attend.StartDate).FirstOrDefault();
            if (modifyPresence != null)
            {
                modifyPresence.Presence = userCreateModel.Presence;

                EditAttendance(modifyPresence);
            }
        }

        public List<Attendance> GetAttendanceByUserId(string id)
        {
            return _serviceAttendance.GetAttendanceByUserId(id);
        }

        public void DeleteData(Guid id)
        {
            _serviceAttendance.DeleteData(id);
        }
    }
}

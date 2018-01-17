using System;
using System.Collections.Generic;
using Business.ServicesInterfaces.Models.PresenceViewModels;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IPresenceService
    {
        void StartLaboratoryBasedOnValue(Guid factionId, int labValue);
        void ApplyModificationsOnUsers(string name, List<UserStatus> selectedStudents);
        List<UserStatus> GetUsersGivenGroup(string name, Guid factionId);
        IReadOnlyList<Presence> GetAllPresences();
        Presence GetPresenceById(Guid id);
        IReadOnlyList<UserStatus> GetAllUsers();
        UserStatus GetUserById(string id);
        IReadOnlyList<UserStatus> GetUsersByFactionId(Guid id);
        List<Attendance> GetAttendanceByUserId(string name);
        void DeleteData(Guid id);
        void CreateFaction(PresenceCreateModel presenceCreateModel);
        bool UserStatusExists(string id);
        IReadOnlyList<Attendance> GetAllAttendances();
        void EditAttendance(Attendance attendance);
        void ModifyPresence(string userId, UserStatusCreateModel userCreateModel);
    }
}

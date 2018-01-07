using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IPresenceService
    {
        void StartLaboratoryBasedOnValue(Guid factionId, int labValue);
        void ApplyModificationsOnUsers(string name, List<UserStatus> selectedStudents);
        List<UserStatus> GetUsersGivenGroup(string name, Guid factionId);
    }
}

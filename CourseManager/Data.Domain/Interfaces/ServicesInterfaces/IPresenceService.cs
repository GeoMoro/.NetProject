using Data.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface IPresenceService
    {
        void StartLaboratoryBasedOnValue(Guid factionId, int labValue);
        void ApplyModificationsOnUsers(string name, List<UserStatus> selectedStudents);
    }
}

using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IUserStatusService
    {
        IReadOnlyList<UserStatus> GetUsersByFactionId(Guid id);
        UserStatus CreateAndReturnLatestUser(string id, Guid factionId);
        void EditFaction(string id, Guid newFaction);
    }
}

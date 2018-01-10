using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IUserStatusService
    {
        IReadOnlyList<UserStatus> GetUsersByFactionId(Guid id);
        //IReadOnlyList<UserStatus> GetUsersByPresence(Guid id);
        UserStatus CreateAndReturnLatestUser(string id, Guid factionId);//, double labMark, double kataMark, bool presence);
        void EditFaction(string id, Guid newFaction);
    }
}

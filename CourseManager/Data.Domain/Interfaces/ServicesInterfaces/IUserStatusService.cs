using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface IUserStatusService
    {
        IReadOnlyList<UserStatus> GetUsersByFactionId(Guid id);
        //IReadOnlyList<UserStatus> GetUsersByPresence(Guid id);
        UserStatus CreateAndReturnLatestUser(string id);//, double labMark, double kataMark, bool presence);
        void EditFaction(string id, Guid newFaction);
    }
}

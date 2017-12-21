using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface IUserStatusService
    {
        IReadOnlyList<UserStatus> GetUsersByLaboratory(Guid id);
        UserStatus CreateAndReturnLatestUser(string id, Guid labId, double labMark, double kataMark, bool presence);
        void EditLaboratory(string id, Guid newLabortory);
    }
}

using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface IUserStatusService
    {
        IReadOnlyList<UserStatus> GetUsersByLaboratory(Guid id);
    }
}

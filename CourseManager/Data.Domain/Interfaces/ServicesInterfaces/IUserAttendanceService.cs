using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface IUserAttendanceService
    {
        List<Guid> GetUsersFromAGroup(string group); 
    }
}

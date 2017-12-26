using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface IUserAttendanceService
    {
        List<Attendance> GetAttendanceByUserId(string name);
        void DeleteData(Guid id);
    }
}

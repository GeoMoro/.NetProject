using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IUserAttendanceService
    {
        List<Attendance> GetAttendanceByUserId(string name);
        void DeleteData(Guid id);
    }
}

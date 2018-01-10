using Data.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Data.Domain.Interfaces
{
    public interface IAttendanceRepository
    {
        IReadOnlyList<Attendance> GetAllAttendances();
        Attendance GetAttendanceById(Guid id);
        void CreateAttendance(Attendance attendance);
        void EditAttendance(Attendance attendance);
        void DeleteAttendance(Attendance attendance);
    }
}

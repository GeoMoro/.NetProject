using Data.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Data.Domain.Interfaces
{
    public interface IAttendanceRepository
    {
        IReadOnlyList<Attendance> GetAllAttendances();
        List<Attendance> GetAttendanceById(string id);
        void CreateAttendance(Attendance attendance);
        void EditAttendance(Attendance attendance);
        void DeleteAttendance(Attendance attendance);
        bool GetCurrentLaboratoryForGivenFaction(string id, int name);
    }
}

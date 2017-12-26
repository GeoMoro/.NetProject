using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;

namespace Business
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly DatabaseContext _databaseService;

        public AttendanceRepository(DatabaseContext databaseService)
        {
            _databaseService = databaseService;
        }

        public IReadOnlyList<Attendance> GetAllAttendances()
        {
            return _databaseService.Attendances.ToList();
        }

        public List<Attendance> GetAttendanceById(string id)
        {
            return _databaseService.Attendances.Where(attendance => attendance.UserId == id).ToList();
        }

        public bool GetCurrentLaboratoryForGivenFaction(string id, int name)
        {
            return _databaseService.Attendances.Where(attendance => attendance.UserId == id && attendance.LaboratoryNumber == name).ToList().Count == 0;
        }

        public void CreateAttendance(Attendance attendance)
        {
            _databaseService.Attendances.Add(attendance);

            _databaseService.SaveChanges();
        }

        public void EditAttendance(Attendance attendance)
        {
            _databaseService.Attendances.Update(attendance);

            _databaseService.SaveChanges();
        }

        public void DeleteAttendance(Attendance attendance)
        {
            _databaseService.Attendances.Remove(attendance);

            _databaseService.SaveChanges();
        }
    }
}

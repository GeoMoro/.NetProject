using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class Attendance
    {
        public Guid Id { get; set; }

        public string UserId {get; set;}

        public Guid LaboratoryId { get; set; }

        public bool Presence { get; set; }

        public DateTime StartDate { get; set; }

        public double LaboratoryMark { get; set; }

        public double KataMark { get; set; }

        public int LaboratoryNumber { get; set; }

        public static Attendance CreateAttendance(int laboratoryNumber, Guid laboratoryId, string userId, double laboratoryMark, double kataMark, bool presence)
        {
            var instance = new Attendance
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.Now
            };
            instance.UpdateAttendance(laboratoryNumber, laboratoryId, userId, laboratoryMark, kataMark, presence);

            return instance;
        }

        private void UpdateAttendance(int laboratoryNumber, Guid laboratoryId, string userId, double laboratoryMark, double kataMark, bool presence)
        {
            LaboratoryNumber = laboratoryNumber;
            LaboratoryId = laboratoryId;
            UserId = userId;
            LaboratoryMark = laboratoryMark;
            KataMark = kataMark;
            Presence = presence;
        }
    }
}

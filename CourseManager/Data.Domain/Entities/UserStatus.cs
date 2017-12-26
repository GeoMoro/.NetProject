using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class UserStatus
    {
        [Key]
        public string Id { get; set; }

        public Guid FactionId { get; set; }
        
        public List<Attendance> Attendance { get; set; }

        public static UserStatus CreateUsersStatus(string id)//, double laboratoryMark, double kataMark, bool presence)
        {
            var instance = new UserStatus
            {
                Id = id,
                Attendance = new List<Attendance>()
            };
            //instance.UpdateUserStatus(laboratoryMark, kataMark, presence);

            return instance;
        }

        //private void UpdateUserStatus(double laboratoryMark, double kataMark, bool presence)
        //{
        //    LaboratoryMark = laboratoryMark;
        //    KataMark = kataMark;
        //    Presence = presence;
        //}
    }
}

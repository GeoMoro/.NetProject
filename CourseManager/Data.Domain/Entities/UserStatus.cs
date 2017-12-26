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

        public static UserStatus CreateUsersStatus(string id)
        {
            var instance = new UserStatus
            {
                Id = id,
                Attendance = new List<Attendance>()
            };

            return instance;
        }
    }
}

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

        public static UserStatus CreateUsersStatus(string id, Guid factionId)
        {
            var instance = new UserStatus
            {
                Id = id,
                FactionId = factionId,
                Attendance = new List<Attendance>()
            };

            return instance;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class UserStatus
    {
        [Key]
        public string Id { get; set; }

        public Guid LaboratoryId { get; set; }

        public double LaboratoryMark { get; set; }

        public double KataMark { get; set; }

        public bool Presence { get; set; }

        public static UserStatus CreatePresence(string id, Guid laboratoryId, double laboratoryMark, double kataMark, bool presence)
        {
            var instance = new UserStatus
            {
                Id = id,
                LaboratoryId = laboratoryId
            };
            instance.UpdateUserStatus(laboratoryMark, kataMark, presence);

            return instance;
        }

        private void UpdateUserStatus(double laboratoryMark, double kataMark, bool presence)
        {
            LaboratoryMark = laboratoryMark;
            KataMark = kataMark;
            Presence = presence;
        }
    }
}

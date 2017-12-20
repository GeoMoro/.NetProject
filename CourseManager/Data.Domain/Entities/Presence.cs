using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class Presence
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public DateTime StartDate { get; set; }

        public List<UserStatus> Students { get; set; }

        public static Presence CreatePresence(string name)
        {
            var instance = new Presence
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.Now
            };
            instance.UpdatePresence(name);

            return instance;
        }

        private void UpdatePresence(string name)
        {
            Name = name;
        }
    }
}
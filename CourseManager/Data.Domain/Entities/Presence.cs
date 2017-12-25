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

        public List<UserStatus> Students { get; set; }

        public static Presence CreatePresence(string name, List<UserStatus> students)
        {
            var instance = new Presence
            {
                Id = Guid.NewGuid()
            };
            instance.UpdatePresence(name, students);

            return instance;
        }

        private void UpdatePresence(string name, List<UserStatus> students)
        {
            Name = name;
            Students = students;
        }
    }
}
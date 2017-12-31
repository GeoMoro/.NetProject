using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class Presence
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<UserStatus> Students { get; set; }

        public static Presence CreatePresence(Guid id, string name, List<UserStatus> students)
        {
            var instance = new Presence
            {
                Id = id
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
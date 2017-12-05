using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Data.Domain.Interfaces;

namespace Data.Domain.Entities
{
    public class Presence
    {
        public Presence()
        {
            //EF
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Laboratory is required")]
        [MinLength(1, ErrorMessage = "Laboratory must have at least 1 character")]
        public string Laboratory { get; set; }

        [Required(ErrorMessage = "A student must be marked as present or not")]
        public bool Present { get; set; }

        public static Presence CreatePresence(string laboratory, bool present)
        {
            var instance = new Presence
            {
                Id = Guid.NewGuid()
            };
            instance.UpdatePresence(laboratory, present);

            return instance;
        }

        private void UpdatePresence(string laboratory, bool present)
        {
            Laboratory = laboratory;
            Present = present;
        }
    }
}
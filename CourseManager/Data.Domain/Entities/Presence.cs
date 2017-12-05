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

        public bool Present { get; set; }
    }
}
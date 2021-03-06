﻿using System.ComponentModel.DataAnnotations;

namespace Business.ServicesInterfaces.Models
{
    public class NewsEditModel
    {
        public NewsEditModel()
        {
            //EF Core
        }

        [Required(ErrorMessage = "A title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A description is required.")]
        [StringLength(2000, ErrorMessage = "Maximum number of characters is 2000!")]
        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public NewsEditModel(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
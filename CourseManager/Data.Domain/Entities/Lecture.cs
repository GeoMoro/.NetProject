using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Data.Domain.Entities
{
    public class Lecture
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "A title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A description is required.")]
        [DataType(DataType.Html)]
        [StringLength(2000, ErrorMessage = "Maximum number of characters is 2000!")]
        public string Description { get; set; }

        public static Lecture CreateLecture(string title, string description)
        {
            var instance = new Lecture { Id = Guid.NewGuid() };
            instance.UpdateLecture(title, description);

            return instance;
        }

        private void UpdateLecture(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}

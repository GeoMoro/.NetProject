using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class Kata
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "A title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A description is required.")]
        [DataType(DataType.Html)]
        [StringLength(2000, ErrorMessage = "Maximum number of characters is 2000!")]
        public string Description { get; set; }

        public static Kata CreateKata(string title, string description)
        {
            var instance = new Kata { Id = Guid.NewGuid() };
            instance.UpdateKata(title, description);

            return instance;
        }

        private void UpdateKata(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}

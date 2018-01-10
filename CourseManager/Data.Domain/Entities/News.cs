using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class News
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "A title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A description is required.")]
        [DataType(DataType.Html)]
        [StringLength(2000, ErrorMessage = "Maximum number of characters is 2000!")]
        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAtDate { get; set; }

        public static News CreateNews(string title, string description, string createdBy)
        {
            var instance = new News { Id = Guid.NewGuid() };
            instance.UpdateNews(title, description, createdBy);

            return instance;
        }

        private void UpdateNews(string title, string description, string createdBy)
        {
            Title = title;
            Description = description;
            CreatedBy = createdBy;
            CreatedAtDate = DateTime.UtcNow;
        }

    }
}
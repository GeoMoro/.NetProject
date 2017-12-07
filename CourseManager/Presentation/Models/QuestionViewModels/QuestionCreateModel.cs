using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class QuestionCreateModel
    {
        [Required(ErrorMessage = "User Id is required.")]
        public Guid UserId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate = DateTime.Now;

        [Required(ErrorMessage = "Topic is required.")]
        [MinLength(1, ErrorMessage = "Topic must have at least 1 character.")]
        [MaxLength(50, ErrorMessage = "Topic cannot exceed 50 characters.")]
        public string Topic { get; set; }

        [Required(ErrorMessage = "Answer Text is required.")]
        [MinLength(1, ErrorMessage = "Answer must have at least 1 character.")]
        [MaxLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters.")]
        public string Text { get; set; }
    }
}
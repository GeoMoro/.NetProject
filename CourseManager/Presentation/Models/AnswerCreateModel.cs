using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class AnswerCreateModel
    {
        [Required(ErrorMessage = "User Id is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Question Id is required.")]
        public Guid QuestionId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AnswerDate = DateTime.Now;

        [Required(ErrorMessage = "Answer Text is required.")]
        [MinLength(1, ErrorMessage = "Answer must have at least 1 character.")]
        [MaxLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters.")]
        public string Text { get; set; }
    }
}
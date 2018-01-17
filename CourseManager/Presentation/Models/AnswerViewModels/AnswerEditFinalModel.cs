using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Models.AnswerViewModels
{
    public class AnswerEditFinalModel
    {
        public AnswerEditFinalModel()
        {
        }

        [DataType(DataType.DateTime)]
        public DateTime AnswerDate = DateTime.Now;

        [Required(ErrorMessage = "Answer Text is required.")]
        [MinLength(1, ErrorMessage = "Answer must have at least 1 character.")]
        [MaxLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters.")]
        public string Text { get; set; }

        public AnswerEditFinalModel(string text)
        {
            Text = text;
        }
    }
}

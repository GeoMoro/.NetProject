using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class Answer
    {
        // ReSharper disable once EmptyConstructor
        public Answer()
        {
            // EF
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Question Id is required.")]
        public Guid QuestionId{ get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AnswerDate { get; set; }

        [Required(ErrorMessage = "Answer Text is required.")]
        [MinLength(1, ErrorMessage = "Answer must have at least 1 character.")]
        [MaxLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters.")]
        public string Text{ get; set; }

        public static Answer CreateAnswer(Guid answerId, Guid questionId, string text)
        {
            var instance = new Answer
            {
                Id = Guid.NewGuid()
            };

            instance.UpdateAnswer(answerId, questionId, text);

            return instance;
        }

        private void UpdateAnswer(Guid answerId, Guid questionId, string text)
        {
            UserId = answerId;
            QuestionId = questionId;
            AnswerDate = DateTime.Now;
            Text = text;
        }
    }
}
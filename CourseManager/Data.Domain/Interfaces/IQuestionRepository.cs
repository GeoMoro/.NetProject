using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface IQuestionRepository
    {
        IReadOnlyList<Question> GetAllQuestions();
        IList<Answer> GetAllAnswersForQuestion(Guid id);
        Question GetQuestionById(Guid id);
        void CreateQuestion(Question question);
        void EditQuestion(Question question);
        void Delete(Question question);
    }
}
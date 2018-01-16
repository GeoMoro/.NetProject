using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface IQuestionService
    {
        IReadOnlyList<Question> GetAllQuestions();
        IList<Answer> GetAllAnswersForQuestion(Guid id);
        Question GetQuestionById(Guid id);
        void CreateQuestion(Question question);
        void EditQuestion(Question question);
        void DeleteQuestion(Question question);
    }
}

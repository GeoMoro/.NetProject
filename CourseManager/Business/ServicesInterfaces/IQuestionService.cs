using System;
using System.Collections.Generic;
using Business.ServicesInterfaces.Models.QuestionViewModels;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IQuestionService
    {
        IReadOnlyList<Question> GetAllQuestions();
        IList<Answer> GetAllAnswersForQuestion(Guid id);
        Question GetQuestionById(Guid id);
        void CreateQuestion(Question question);
        void EditQuestion(Question question);
        void DeleteQuestion(Question question);
        void CreateQuestion(QuestionCreateModel questionCreateModel);
        bool CheckIfQuestionExists(Guid id);
    }
}

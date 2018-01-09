using System;
using System.Collections.Generic;
using Business.ServicesInterfaces.Models.QuestionViewModels;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IQuestionService
    {
        void GetQuestionsWithAnswers();
        IReadOnlyList<Question> GetAllQuestions();
        Question GetQuestionById(Guid idValue);
        void Create(QuestionCreateModel questionCreateModel);
        void Edit(Question questionToBeEdited, QuestionEditModel questionEditModel);
        void Delete(Question question);
        bool VerifyIfQuestionExists(Guid id);
    }
}

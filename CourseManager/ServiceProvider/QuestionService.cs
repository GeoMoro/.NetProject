using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.QuestionViewModels;
using Data.Domain.Entities;
using Data.Domain.Interfaces;

namespace ServicesProvider
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _repository;

        public QuestionService(IQuestionRepository repository)
        {
            _repository = repository;
        }

        public IList<Answer> GetAllAnswersForQuestion(Guid id)
        {
            return _repository.GetAllAnswersForQuestion(id);
        }

        public IReadOnlyList<Question> GetAllQuestions()
        {
            return _repository.GetAllQuestions();
        }

        public Question GetQuestionById(Guid id)
        {
            return _repository.GetQuestionById(id);
        }

        public void CreateQuestion(Question question)
        {
            _repository.CreateQuestion(question);
        }

        public void EditQuestion(Question question)
        {
            _repository.EditQuestion(question);
        }

        public void DeleteQuestion(Question question)
        {
            _repository.DeleteQuestion(question);
        }

        public void CreateQuestion(QuestionCreateModel questionCreateModel)
        {
            _repository.CreateQuestion(
                Question.CreateQuestion(
                    questionCreateModel.UserId,
                    questionCreateModel.Topic,
                    questionCreateModel.Text
                )
            );
        }

        public bool CheckIfQuestionExists(Guid id)
        {
            return _repository.GetAllQuestions().Any(question => question.Id == id);
        }
    }
}

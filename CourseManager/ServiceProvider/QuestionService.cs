using System;
using System.Collections.Generic;
using System.Linq;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.QuestionViewModels;
using Data.Domain.Entities;
using Data.Domain.Interfaces;

namespace ServicesProvider
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public void GetQuestionsWithAnswers()
        {
            foreach (var item in _questionRepository.GetAllQuestions())
            {
                item.Answers = _questionRepository.GetAllAnswersForQuestion(item.Id);
            }
        }

        public IReadOnlyList<Question> GetAllQuestions()
        {
            return _questionRepository.GetAllQuestions();
        }

        public Question GetQuestionById(Guid idValue)
        {
            return _questionRepository.GetQuestionById(idValue);
        }

        public void Create(QuestionCreateModel questionCreateModel)
        {
            _questionRepository.CreateQuestion(
                Question.CreateQuestion(
                    questionCreateModel.UserId,
                    questionCreateModel.Topic,
                    questionCreateModel.Text//,
                    // new List<string> { "FE", "BU" }
                )
            );
        }

        public void Edit(Question questionToBeEdited, QuestionEditModel questionEditModel)
        {
            questionToBeEdited.UserId = questionEditModel.UserId;
            questionToBeEdited.CreatedDate = questionEditModel.CreatedDate;
            questionToBeEdited.Topic = questionEditModel.Topic;
            questionToBeEdited.Text = questionEditModel.Text;

            _questionRepository.EditQuestion(questionToBeEdited);
        }

        public void Delete(Question question)
        {
            _questionRepository.DeleteQuestion(question);
        }

        public bool VerifyIfQuestionExists(Guid id)
        {
            return GetAllQuestions().Any(question => question.Id == id);
        }  
    }
}

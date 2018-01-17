using System;
using System.Collections.Generic;
using System.Linq;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.AnswerViewModels;
using Data.Domain.Entities;
using Data.Domain.Interfaces;

namespace ServicesProvider
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _repository;

        public AnswerService(IAnswerRepository repository)
        {
            _repository = repository;
        }

        public List<Answer> GetAllAnswers()
        {
            return _repository.GetAllAnswers();
        }

        public List<Answer> GetAllAnswersForGivenQuestion(Guid qid)
        {
            return _repository.GetAllAnswersForGivenQuestion(qid);
        }

        public List<Answer> GetAllAnswersForGivenUserId(Guid uid)
        {
            return _repository.GetAllAnswersForGivenUserId(uid);
        }

        public Answer GetAnswerById(Guid id)
        {
            return _repository.GetAnswerById(id);
        }

        public void CreateAnswer(Answer answer)
        {
            _repository.CreateAnswer(answer);
        }

        public void CreateAnswerForGivenQuestion(Guid qid, Answer answer)
        {
            _repository.CreateAnswerForGivenQuestion(qid, answer);
        }

        public void EditAnswer(Answer answer)
        {
            _repository.EditAnswer(answer);
        }

        public void DeleteAnswer(Answer answer)
        {
            _repository.DeleteAnswer(answer);
        }

        public void CreateAnswer(Guid? id, AnswerCreateModel answerCreateModel)
        {
            _repository.CreateAnswer(
                Answer.CreateAnswer(
                    answerCreateModel.UserId,
                    id.Value,
                    answerCreateModel.Text
                )
            );
        }

        public void CreateNew(Guid? qid, Guid? uid, String text)
        {
            _repository.CreateAnswer(
                Answer.CreateAnswer(
                    uid.Value,
                    qid.Value,
                    text
                )
            );
        }

        public void CreateNewAnswer(Guid? uid, Guid? qid, string qtext)
        {
            if (qtext != null)
            {
                _repository.CreateAnswer(
                    Answer.CreateAnswer(
                        uid.Value,
                        qid.Value,
                        qtext
                    )
                );
            }
        }

        public bool CheckIfAnswerExists(Guid id)
        {
            return _repository.GetAllAnswers().Any(answer => answer.Id == id);
        }
    }
}

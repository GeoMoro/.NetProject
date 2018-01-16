using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface IAnswerRepository
    {
        List<Answer> GetAllAnswers();
        List<Answer> GetAllAnswersForGivenQuestion(Guid qid);
        List<Answer> GetAllAnswersForGivenUserId(Guid uid);
        Answer GetAnswerById(Guid id);
        void CreateAnswer(Answer answer);
        void CreateAnswerForGivenQuestion(Guid qid, Answer answer);
        void EditAnswer(Answer answer);
        void DeleteAnswer(Answer answer);
    }
}
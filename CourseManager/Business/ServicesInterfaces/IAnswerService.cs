using System;
using System.Collections.Generic;
using Business.ServicesInterfaces.Models.AnswerViewModels;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IAnswerService
    {
        List<Answer> GetAllAnswers();
        List<Answer> GetAllAnswersForGivenQuestion(Guid qid);
        List<Answer> GetAllAnswersForGivenUserId(Guid uid);
        Answer GetAnswerById(Guid id);
        void CreateAnswer(Answer answer);
        void CreateAnswerForGivenQuestion(Guid qid, Answer answer);
        void EditAnswer(Answer answer);
        void DeleteAnswer(Answer answer);
        void CreateAnswer(Guid? id, AnswerCreateModel answerCreateModel);
        void CreateNew(Guid? qid, Guid? uid, String text);
        void CreateNewAnswer(Guid? uid, Guid? qid, string qtext);
        bool CheckIfAnswerExists(Guid id);
    }
}

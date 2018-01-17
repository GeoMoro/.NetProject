using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;

namespace Business
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly DatabaseContext _databaseService;

        public AnswerRepository(DatabaseContext databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Answer> GetAllAnswers()
        {
            return _databaseService.Answers.ToList();
        }

        public List<Answer> GetAllAnswersForGivenQuestion(Guid qid)
        {
            return _databaseService.Answers.Where(answer => answer.QuestionId == qid).ToList();
        }

        public List<Answer> GetAllAnswersForGivenUserId(Guid uid)
        {
            return _databaseService.Answers.Where(answer => answer.UserId == uid).ToList();
        }

        public Answer GetAnswerById(Guid id)
        {
            return _databaseService.Answers.SingleOrDefault(answer => answer.Id == id);
        }

        public void CreateAnswer(Answer answer)
        {
            _databaseService.Answers.Add(answer);

            _databaseService.SaveChanges();
        }

        public void CreateAnswerForGivenQuestion(Guid qid, Answer answer)
        {
            answer.QuestionId = qid;

            _databaseService.Answers.Add(answer);

            _databaseService.SaveChanges();
        }

        public void EditAnswer(Answer answer)
        {
            _databaseService.Answers.Update(answer);

            _databaseService.SaveChanges();
        }

        public void DeleteAnswer(Answer answer)
        {
            _databaseService.Answers.Remove(answer);

            _databaseService.SaveChanges();
        }
        
    }
}
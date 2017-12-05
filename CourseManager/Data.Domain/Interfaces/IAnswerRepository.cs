using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface IAnswerRepository
    {
        IReadOnlyList<Answer> GetAllAnswers();
        Answer GetAnswerById(Guid id);
        void CreateAnswer(Answer answer);
        void EditAnswer(Answer answer);
        void DeleteAnswer(Answer answer);
    }
}
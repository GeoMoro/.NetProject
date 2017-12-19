using System;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface IQuestionService
    {
        void DeleteQuestion(Question question);
    }
}

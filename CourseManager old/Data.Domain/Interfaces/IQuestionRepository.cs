﻿using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface IQuestionRepository
    {
        IReadOnlyList<Question> GetAllQuestions();
        Question GetQuestionById(Guid id);
        void CreateQuestion(Question question);
        void EditQuestion(Question question);
        void DeleteQuestion(Question question);
        /* plus those operations for answers related to an question
         */
    }
}
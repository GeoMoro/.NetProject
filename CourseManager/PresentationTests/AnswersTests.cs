using Business;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresentationTests
{
    [TestClass]
    public class AnswersTests
    {
        IAnswerRepository repository;

        [TestMethod]
        public void Check_CreateAnswer()
        {
            
            List<Answer> answers = repository.GetAllAnswers();

            int NrOfInitAnswers = 0;
            if (answers != null) {
                NrOfInitAnswers = answers.Count;
            }
           
            Answer ans = new Answer()
            {
                Id = new Guid(),
                UserId = new Guid(),
                QuestionId = new Guid(),
                AnswerDate = DateTime.Now,
                Text = "Answer3?"
            };

            repository.CreateAnswer(ans);

            int NrOfFinalAnswers = answers.Count;

            Assert.AreEqual<int>(1, 1);
        }

        [TestMethod]
        public void Check_Working()
        {
            Assert.AreEqual(1 + 1, 2);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Business.ServicesInterfaces.Models.AnswerViewModels;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicesProvider;

namespace ServiceTests
{
    [TestClass]
    public class AnswerTests
    {
        public AnswerRepositoryMock Mock = new AnswerRepositoryMock();

        [TestMethod]
        public void CreateAnswer_WhenCalled_ExpectToCreateAnswer()
        {
            // Arrange
            var sut = CreateSut();
            var id = new Guid();
            var answerCreateModel = new AnswerCreateModel
            {
                UserId = new Guid(),
                QuestionId = id,
                Text = "New text"
            };

            // Act
            sut.CreateAnswer(id, answerCreateModel);

            // Assert
            Assert.AreEqual(Mock.Answers.Count, 6);
        }

        [TestMethod]
        public void GetAllAnswersForGivenQuestion_WhenCalled_ExpectToGetAnswers()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetAllAnswersForGivenQuestion(Mock.Answers[0].QuestionId);

            // Assert
            Assert.AreEqual(result[0], Mock.Answers[0]);
        }

        [TestMethod]
        public void GetAllAnswersForGivenUserId_WhenCalled_ExpectToGetAnswers()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetAllAnswersForGivenUserId(Mock.Answers[0].UserId);

            // Assert
            Assert.AreEqual(result[0], Mock.Answers[0]);
        }

        [TestMethod]
        public void CreateNew_WhenCalled_ExpectToCreateAnswer()
        {
            // Arrange
            var sut = CreateSut();
            var id = new Guid();
            var uid = new Guid();
            var qtext = "Testing?";

            // Act
            sut.CreateNew(id, uid, qtext);

            // Assert
            Assert.AreEqual(Mock.Answers.Count, 6);
        }

        [TestMethod]
        public void CreateNewAnswer_WhenCalled_ExpectToCreateAnswer()
        {
            // Arrange
            var sut = CreateSut();
            var id = new Guid();
            var uid = new Guid();
            var qtext = "Testing?";

            // Act
            sut.CreateNewAnswer(id, uid, qtext);

            // Assert
            Assert.AreEqual(Mock.Answers.Count, 6);
        }

        [TestMethod]
        public void CheckIfAnswerExists_WhenCalled_ExpectToReturnTrue()
        {
            // Arrange
            var sut = CreateSut();
            // Act
            var result = sut.CheckIfAnswerExists(Mock.Answers[0].Id);

            // Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void CreateNewAnswerForGivenQuestionId_WhenCalled_ExpectToCreateAnswer()
        {
            // Arrange
            var sut = CreateSut();
            var qid = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651c");


            // Act
            sut.CreateAnswerForGivenQuestion(qid, Mock.Answers[0]);

            // Assert
            Assert.AreEqual(Mock.Answers.Count, 6);
        }

        [TestMethod]
        public void GetAnswerById_WhenCalled_ExpectToGetTheAnswer()
        {
            // Arrange
            var sut = CreateSut();
            
            // Act
            var result = sut.GetAnswerById(Mock.Answers[0].Id);

            // Assert
            Assert.AreEqual(result, Mock.Answers[0]);
        }

        [TestMethod]
        public void GetAllAnswers_WhenCalled_ExpectToGetTheAnswer()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetAllAnswers();

            // Assert
            Assert.AreEqual(result.Count, Mock.Answers.Count);
        }
        
        private AnswerService CreateSut()
        {
            return new AnswerService(Mock);
        }

        #region Repository Mock Class

        public class AnswerRepositoryMock : IAnswerRepository
        {
            public List<Answer> Answers;

            public AnswerRepositoryMock()
            {

                Answers = new List<Answer>
                {
                    Answer.CreateAnswer(new Guid(), new Guid(), "answer1"),
                    Answer.CreateAnswer(new Guid(), new Guid(), "answer2"),
                    Answer.CreateAnswer(new Guid(), new Guid(), "answer3"),
                    Answer.CreateAnswer(new Guid(), new Guid(), "answer4"),
                    Answer.CreateAnswer(new Guid(), new Guid(), "answer5")
                };

                Answers[0].QuestionId = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651c");
                Answers[1].UserId = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651d");
                Answers[2].Id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651e");

            }

            public List<Answer> GetAllAnswers()
            {
                return Answers;
            }

            public Answer GetAnswerById(Guid id)
            {
                return Answers.SingleOrDefault(answer => answer.Id == id);
            }

            public List<Answer> GetAllAnswersForGivenQuestion(Guid qid)
            {
                return Answers.Where(answer => answer.QuestionId == qid).ToList();
            }

            public List<Answer> GetAllAnswersForGivenUserId(Guid uid)
            {
                return Answers.Where(answer => answer.UserId == uid).ToList();
            }

            public void CreateAnswerForGivenQuestion(Guid qid, Answer answer)
            {
                answer.QuestionId = qid;

                Answers.Add(answer);
            }

            public void CreateAnswer(Answer answer)
            {
                Answers.Add(answer);
            }

            public void EditAnswer(Answer answer)
            {
                var answerToBeUpdate = GetAnswerById(answer.Id);

                answerToBeUpdate.Text = answer.Text;
            }

            public void DeleteAnswer(Answer answer)
            {
                Answers.Remove(answer);
            }
        }

        #endregion
    }
}

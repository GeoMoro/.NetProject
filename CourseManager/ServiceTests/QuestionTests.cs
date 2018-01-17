using System;
using System.Collections.Generic;
using System.Linq;
using Business.ServicesInterfaces.Models.QuestionViewModels;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicesProvider;

namespace ServiceTests
{
    [TestClass]
    public class QuestionTests
    {
        public QuestionRepositoryMock Mock = new QuestionRepositoryMock();

        [TestMethod]
        public void CreateQuestion_WhenCalled_ExpectToCreateAQuestion()
        {
            // Arrange
            var sut = CreateSut();
            var questionCreateModel = new QuestionCreateModel
            {
                UserId = new Guid(),
                Topic = "New topic",
                Text = "New text"
            };

            // Act
            sut.CreateQuestion(questionCreateModel);

            // Assert
            Assert.AreEqual(Mock.Questions.Count, 6);
        }

        [TestMethod]
        public void EditQuestion_WhenCalled_ExpectToEditTheQuestion()
        {
            // Arrange
            var sut = CreateSut();
            var question = Mock.Questions[0];
            question.Topic = "DADA";
            question.Text = "NEW TEXTT";

            // Act
            sut.EditQuestion(question);
              
            // Assert
            Assert.AreEqual(Mock.Questions[0].Topic, "DADA");
        }

        [TestMethod]
        public void GetAllAnswers_WhenCalled_ExpectToGetAnswers()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetAllAnswersForQuestion(Mock.Questions[0].Id);

            // Assert
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void GetQuestionId_WhenCalled_ExpectToGetTheQuestion()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var question = sut.GetQuestionById(Mock.Questions[0].Id);

            // Assert
            Assert.AreEqual(Mock.Questions[0], question);
        }

        [TestMethod]
        public void GetAllQuestion_WhenCalled_ExpectToGetAllQuestions()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetAllQuestions();

            // Assert
            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public void CheckIfExists_WhenCalled_ExpectToGetTrue()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.CheckIfQuestionExists(Mock.Questions[0].Id);

            // Assert
            Assert.AreEqual(true, result);
        }


        [TestMethod]
        public void GetQuestionById_WhenCalled_ExpectToGetTheQuestion()
        {
            // Arrange
            var sut = CreateSut();
            var id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651a");
            Mock.Questions[0].Id = id;
            
            // Act
            var result = sut.GetQuestionById(id);

            // Assert
            Assert.AreEqual(result.Topic, Mock.Questions[0].Topic);
        }


        private QuestionService CreateSut()
        {
            return new QuestionService(Mock);
        }

        #region Repository Mock Class

        public class QuestionRepositoryMock : IQuestionRepository
        {
            public List<Question> Questions;

            public QuestionRepositoryMock()
            {
                Questions = new List<Question>
                {
                    Question.CreateQuestion(new Guid(), "Topic1", "Text1"),
                    Question.CreateQuestion(new Guid(), "Topic2", "Text2"),
                    Question.CreateQuestion(new Guid(), "Topic3", "Text3"),
                    Question.CreateQuestion(new Guid(), "Topic4", "Text4"),
                    Question.CreateQuestion(new Guid(), "Topic5", "Text5")
                };

                Questions[0].Id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651a");
            }

            public IReadOnlyList<Question> GetAllQuestions()
            {
                return Questions;
            }

            public IList<Answer> GetAllAnswersForQuestion(Guid id)
            {
                var results = (from questions in Questions
                    from answers in questions.Answers
                    where answers.QuestionId == id
                    select answers);

                return results.ToList();
            }

            public Question GetQuestionById(Guid id)
            {
                return Questions.SingleOrDefault(answer => answer.Id == id);
            }

            public void CreateQuestion(Question question)
            {
                Questions.Add(question);
            }

            public void EditQuestion(Question question)
            {
                var questionToBeUpdate = GetQuestionById(question.Id);

                questionToBeUpdate.Text = question.Text;
                questionToBeUpdate.Topic = question.Topic;
            }

            public void DeleteQuestion(Question question)
            {
                Questions.Remove(question);
            }
        }

        #endregion
    }
}

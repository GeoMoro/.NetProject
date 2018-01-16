using Business;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationTests
{
    [TestClass]
    public class AnswersTests
    {
        public class AnswerRepositoryMock : IAnswerRepository
        {
            public List<Answer> Answers = new List<Answer>();

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

            public List<Answer> GetAllAnswersForGivenQuestion(Guid qid)
            {
                return Answers.Where(answer => answer.QuestionId == qid).ToList();
            }

            public List<Answer> GetAllAnswersForGivenUserId(Guid uid)
            {
                return Answers.Where(answer => answer.UserId == uid).ToList();
            }

            public Answer GetAnswerById(Guid id)
            {

                return Answers.SingleOrDefault(answer => answer.Id == id);
            }

            public void CreateAnswer(Answer answer)
            {
                Answers.Add(answer);
            }

            public void CreateAnswerForGivenQuestion(Guid qid, Answer answer)
            {
                answer.QuestionId = qid;
                Answers.Add(answer);
            }


            public void EditAnswer(Answer answer)
            {
                for (int i = 0; i < Answers.Count; i++)
                {
                    if (Answers[i].Id == answer.Id)
                    {
                        Answers[i].UserId = answer.UserId;
                        Answers[i].QuestionId = answer.QuestionId;
                        Answers[i].Text = answer.Text;
                        Answers[i].AnswerDate = answer.AnswerDate;
                    }
                }
            }

            public void DeleteAnswer(Answer answer)
            {
                Answers.Remove(answer);
            }
        }

        AnswerRepositoryMock repository = new AnswerRepositoryMock();

        [TestMethod]
        public void Check_GetAll()
        {
            List<Answer> Answerss = repository.GetAllAnswers();
            Assert.AreEqual(Answerss.Count, 5);
        }

        [TestMethod]
        public void Check_GetAllAnswersForGivenQuestion()
        {
            Guid id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651c");
            List<Answer> Answerss = repository.GetAllAnswersForGivenQuestion(id);
            Assert.AreEqual(Answerss[0].Text, "answer1");
        }

        [TestMethod]
        public void Check_GetAllAnswersForGivenUserId()
        {
            Guid id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651d");
            List<Answer> Answerss = repository.GetAllAnswersForGivenUserId(id);
            Assert.AreEqual(Answerss.Count, 1);
        }


        [TestMethod]
        public void Check_GetAnswerById()
        {
            Guid id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651e");
            Answer ans = repository.GetAnswerById(id);
            Assert.AreEqual(ans.Text, "answer3");
        }

        [TestMethod]
        public void Check_CreateAnswer()
        {
            Answer ans = new Answer()
            {
                Id = new Guid(),
                UserId = new Guid(),
                QuestionId = new Guid(),
                AnswerDate = DateTime.Now,
                Text = "Answer6?"
            };

            repository.CreateAnswer(ans);
            Assert.AreEqual(repository.Answers.Count, 6);
        }

        [TestMethod]
        public void Check_CreateAnswerForGivenQuestion()
        {
            Guid id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651e");

            Answer ans = new Answer()
            {
                Id = new Guid(),
                UserId = new Guid(),
                QuestionId = new Guid(),
                AnswerDate = DateTime.Now,
                Text = "Answer6?"
            };

            repository.CreateAnswerForGivenQuestion(id, ans);

            ans.QuestionId = id;

            Assert.AreEqual(repository.Answers[5], ans);
        }

        [TestMethod]
        public void Check_EditAnswer()
        {
            Guid id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651e");  // answer3

            Answer ans = repository.GetAnswerById(id);

            ans.Text = "answer3Edited";

            repository.EditAnswer(ans);

            Answer rezAns = repository.GetAnswerById(id);

            Assert.AreEqual(rezAns.Text, "answer3Edited");
        }


        [TestMethod]
        public void Check_DeleteAnswer()
        {
            Guid id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651e"); // answer3
            Answer ans = repository.GetAnswerById(id);
            repository.DeleteAnswer(ans);

            Assert.AreEqual(repository.Answers.Count, 4);
        }
        
    }
}

﻿using Business;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationTests
{
    [TestClass]
    public class QuestionsTests
    {
        public class QuestionsTestsRepositoryMock : IQuestionService
        {
            public List<Question> Questions = new List<Question>();

            public QuestionsTestsRepositoryMock()
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
                return Questions.ToList();
            }

            public IList<Answer> GetAllAnswersForQuestion(Guid id)
            {
                List<Answer> Answers = new List<Answer>();
                return Answers;
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
                for (int i = 0; i < Questions.Count; i++)
                {
                    if (Questions[i].Id == question.Id)
                    {
                        Questions[i].UserId = question.UserId;
                        Questions[i].CreatedDate = question.CreatedDate;
                        Questions[i].Topic = question.Topic;
                        Questions[i].Text = question.Text;
                        Questions[i].Answers = question.Answers;
                    }
                }
            }

            public void DeleteQuestion(Question question)
            {
                Questions.Remove(question);
            }
            
        }

        QuestionsTestsRepositoryMock mock = new QuestionsTestsRepositoryMock();

        [TestMethod]
        public void Check_GetAll()
        {
            IReadOnlyList<Question> Questions = mock.GetAllQuestions();
            Assert.AreEqual(Questions.Count, 5);
        }
        
       
        
        [TestMethod]
        public void Check_GetAllAnswersForQuestion()
        {
            Guid id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651a");
            IList<Answer> Answerss = mock.GetAllAnswersForQuestion(id);
            Assert.AreEqual(Answerss.Count, 0);
        }

        
        [TestMethod]
        public void Check_GetQuestionById()
        {
            Guid id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651a");
            Question ans = mock.GetQuestionById(id);
            Assert.AreEqual(ans.Topic, "Topic1");
        }
        
        [TestMethod]
        public void Check_CreateQuestion()
        {
            Question qu = new Question()
            {
                Id = new Guid(),
                UserId = new Guid(),
                CreatedDate = DateTime.Now,
                Topic = "Topic6",
                Text = "Text6"
            };

            mock.CreateQuestion(qu);

            Assert.AreEqual(mock.Questions.Count, 6);

            mock.DeleteQuestion(qu);

        }
        
        
        [TestMethod]
        public void Check_EditQuestion()
        {
            Guid id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651a");  // answer3

            Question qu = mock.GetQuestionById(id);

            Question qucopy = qu;

            qu.Text = "Text1Edited";

            mock.EditQuestion(qu);

            Question rezQu = mock.GetQuestionById(id);

            Assert.AreEqual(rezQu.Text, "Text1Edited");

            mock.EditQuestion(qucopy);
        }

        
        [TestMethod]
        public void Check_DeleteQuestion()
        {
            Guid id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651a"); // answer3

            Question qu = mock.GetQuestionById(id);

            mock.DeleteQuestion(qu);

            Assert.AreEqual(mock.Questions.Count, 4);

            mock.DeleteQuestion(qu);
        }
        
    }
}
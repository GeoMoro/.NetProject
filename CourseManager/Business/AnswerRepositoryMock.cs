using Data.Domain.Entities;
using Data.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
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

          /*  Answer answer1 = Answer.CreateAnswer(new Guid(), new Guid(), "answer1");
            Answer answer2 = Answer.CreateAnswer(new Guid(), new Guid(), "answer2");
            Answer answer3 = Answer.CreateAnswer(new Guid(), new Guid(), "answer3");
            Answer answer4 = Answer.CreateAnswer(new Guid(), new Guid(), "answer4");
            Answer answer5 = Answer.CreateAnswer(new Guid(), new Guid(), "answer5");
            
            Answers.Add(answer1);
            Answers.Add(answer2);
            Answers.Add(answer3);
            Answers.Add(answer4);
            Answers.Add(answer5);*/
        }

        public List<Answer> GetAllAnswers()
        {
            return Answers;
        }

        public List<Answer> GetAllAnswersForGivenQuestion(Guid qid)
        {
            List<Answer> ans = new List<Answer>();
            foreach(Answer a in Answers)
            {
                if (a.QuestionId == qid)
                {
                    ans.Add(a);
                }
            }
            return ans;
        }

        public List<Answer> GetAllAnswersForGivenUserId(Guid uid)
        {
            List<Answer> ans = new List<Answer>();
            foreach (Answer a in Answers)
            {
                if (a.UserId == uid)
                {
                    ans.Add(a);
                }
            }
            return ans;
        }

        public Answer GetAnswerById(Guid id)
        {
            Answer ans = new Answer();

            foreach (Answer a in Answers)
            {
                if (a.Id == id)
                {
                    ans.UserId = a.UserId;
                    ans.QuestionId = a.QuestionId;
                    ans.Text = a.Text;
                    ans.AnswerDate = a.AnswerDate;
                    ans.Id = a.Id;

                }
            }
            return ans;
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
            var answerToBeUpdate = GetAnswerById(answer.Id);

            answerToBeUpdate.UserId = answer.UserId;
            answerToBeUpdate.QuestionId = answer.QuestionId;
            answerToBeUpdate.Text = answer.Text;
            answerToBeUpdate.AnswerDate = answer.AnswerDate;
        }


        public void DeleteAnswer(Answer answer)
        {
            Answers.Remove(answer);
        }
    }
}

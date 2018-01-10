using Data.Domain.Entities;
using Data.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class AnswerRepositoryMock: IAnswerRepository
    {
        public List<Answer> Answers;

        public AnswerRepositoryMock()
        {

            Answer answer1 = Answer.CreateAnswer(new Guid(), new Guid(), "answer1");
            Answer answer2 = Answer.CreateAnswer(new Guid(), new Guid(), "answer2");
            Answer answer3 = Answer.CreateAnswer(new Guid(), new Guid(), "answer3");
            Answer answer4 = Answer.CreateAnswer(new Guid(), new Guid(), "answer4");
            Answer answer5 = Answer.CreateAnswer(new Guid(), new Guid(), "answer5");
            
            Answers.Add(answer1);
            Answers.Add(answer2);
            Answers.Add(answer3);
            Answers.Add(answer4);
            Answers.Add(answer5);
        }

        public IReadOnlyList<Answer> GetAllAnswers()
        {
            return Answers;
        }

        public Lecture GetLectureById(Guid id)
        {
            return Lectures.SingleOrDefault(lecture => lecture.Id.Equals(id));
        }

        public Lecture GetLectureInfoByDetails(string title, string description)
        {
            return Lectures.SingleOrDefault(lecture => lecture.Title == title && lecture.Description == description);
        }

        public void CreateLecture(Lecture lecture)
        {
            Lectures.Add(lecture);
        }

        public void EditLecture(Lecture lecture)
        {
            var lectureToBeUpdate = GetLectureById(lecture.Id);

            lectureToBeUpdate.Title = lecture.Title;
            lectureToBeUpdate.Description = lecture.Description;
        }

        public void DeleteLecture(Lecture lecture)
        {
            Lectures.Remove(lecture);
        }
    }
}

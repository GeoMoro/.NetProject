using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;

namespace Business
{
    public class LectureRepository : ILectureRepository
    {
        private readonly DatabaseContext _databaseService;

        public LectureRepository(DatabaseContext databaseService)
        {
            _databaseService = databaseService;
        }

        public Lecture GetLectureInfoByDetails(string title, string description)
        {
            return _databaseService.Lectures.SingleOrDefault(lecture => lecture.Title == title && lecture.Description == description);
        }

        public IReadOnlyList<Lecture> GetAllLectures()
        {
            return _databaseService.Lectures.ToList();
        }

        public Lecture GetLectureById(Guid id)
        {
            return _databaseService.Lectures.SingleOrDefault(lecture => lecture.Id == id);
        }

        public void CreateLecture(Lecture lecture)
        {
            _databaseService.Lectures.Add(lecture);

            _databaseService.SaveChanges();
        }

        public void EditLecture(Lecture lecture)
        {
            _databaseService.Lectures.Update(lecture);

            _databaseService.SaveChanges();
        }

        public void DeleteLecture(Lecture lecture)
        {
            _databaseService.Lectures.Remove(lecture);

            _databaseService.SaveChanges();
        }
    }
}

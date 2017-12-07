using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface ILectureRepository
    {
        IReadOnlyList<Lecture> GetAllLectures();
        Lecture GetLectureById(Guid id);
        void CreateLecture(Lecture lecture);
        void EditLecture(Lecture lecture);
        void DeleteLecture(Lecture lecture);
    }
}

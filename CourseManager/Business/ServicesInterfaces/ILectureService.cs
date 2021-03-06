﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Business.ServicesInterfaces.Models.LectureViewModels;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface ILectureService
    {
        List<string> GetFiles(Guid id);
        List<string> GetFilesBasedOnDetails(string title, string description);
        Task CreateLecture(LectureCreateModel lectureToBeCreated);
        Lecture GetLectureInfoByDetails(string title, string description);
        void DeleteFilesForGivenId(Guid id);
        Stream SearchLecture(Guid lectureId, string fileName);
        void DeleteFile(string fileName, Guid? givenId);
        Lecture GetLectureById(Guid idValue);
        IReadOnlyList<Lecture> GetAllLectures();
        bool CheckIfLecturesExists(Guid id);
        Task Edit(Guid id, Lecture lectureEdited, LectureEditModel lectureModel);
        void DeleteLecture(Lecture lecture);
        void DeleFromPath(Guid id);
    }
}

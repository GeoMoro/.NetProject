using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace ServicesProvider
{
    public class LectureService : ILectureService
    {
        private readonly ILectureRepository _repository;

        public LectureService(ILectureRepository repository)
        {
            _repository = repository;
        }

        public List<string> GetFiles(Guid id)
        {
            var currentLecture = _repository.GetLectureById(id);
            var fileList = new List<string>();
            string path = Directory.GetCurrentDirectory() + "\\wwwroot\\Lectures\\" + id;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var files in Directory.GetFiles(path))
            {
                fileList.Add(Path.GetFileName(files));
            }

            return fileList;
        }

        public List<string> GetFilesBasedOnDetails(string title, string description)
        {
            var currentLecture = _repository.GetLectureInfoByDetails(title, description);
            var fileList = new List<string>();
            string path = Directory.GetCurrentDirectory() + "\\wwwroot\\Lectures\\" + currentLecture.Id;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var files in Directory.GetFiles(path))
            {
                fileList.Add(Path.GetFileName(files));
            }

            return fileList;
        }

    }
}

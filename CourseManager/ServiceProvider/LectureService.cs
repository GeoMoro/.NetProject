using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace ServicesProvider
{
    public class LectureService : ILectureService
    {
        private readonly ILectureRepository _repository;
        private readonly IHostingEnvironment _env;

        public LectureService(ILectureRepository repository, IHostingEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public List<string> GetFiles(Guid id)
        {
            var currentLecture = _repository.GetLectureById(id);
            var fileList = new List<string>();
            var path = Directory.GetCurrentDirectory() + "\\wwwroot\\Lectures\\" + id;

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
            var path = Directory.GetCurrentDirectory() + "\\wwwroot\\Lectures\\" + currentLecture.Id;

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

        public void DeleteFilesForGivenId(Guid id)
        {
            var lecture = _repository.GetLectureById(id);

            var searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + id);
            if (Directory.Exists(searchedPath))
            {
                Directory.Delete(searchedPath, true);
            }

            _repository.DeleteLecture(lecture);
        }

        public void DeleteSpecificFiles(string fileName, Guid? givenId)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + givenId.Value + "/" + fileName);
            if (File.Exists(searchedPath))
            {
                File.Delete(searchedPath);
            }
        }

        public Stream Download(Guid lectureId, string fileName)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + lectureId + "/" + fileName);
            Stream file = new FileStream(searchedPath, FileMode.Open);

            return file;
        }
    }
}

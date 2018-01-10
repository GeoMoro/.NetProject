using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.LectureViewModels;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;

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

        public async Task CreateLecture(LectureCreateModel lectureCreateModel)
        {
            _repository.CreateLecture(Lecture.CreateLecture(
                lectureCreateModel.Title,
                lectureCreateModel.Description)
            );

            var currentLecture = GetLectureInfoByDetails(lectureCreateModel.Title, lectureCreateModel.Description);

            if (lectureCreateModel.File != null)
            {
                foreach (var file in lectureCreateModel.File)
                {
                    if (file.Length > 0)
                    {
                        string path = Path.Combine(_env.WebRootPath, "Lectures/" + currentLecture.Id);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        // string extension = lectureCreateModel.Title + "." + Path.GetExtension(file.FileName).Substring(1);

                        using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }

        public Lecture GetLectureInfoByDetails(string title, string description)
        {
            return _repository.GetLectureInfoByDetails(title, description);
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

        public void DeleteFile(string fileName, Guid? givenId)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + givenId.Value + "/" + fileName);

            if (File.Exists(searchedPath))
            {
                File.Delete(searchedPath);
            }
        }

        public Lecture GetLectureById(Guid idValue)
        {
            return _repository.GetLectureById(idValue);
        }

        public IReadOnlyList<Lecture> GetAllLectures()
        {
            return _repository.GetAllLectures();
        }

        public bool CheckIfLecturesExists(Guid id)
        {
            return _repository.GetAllLectures().Any(e => e.Id == id);
        }

        public async Task Edit(Guid id, Lecture lectureEdited, LectureEditModel lectureModel)
        {
            _repository.EditLecture(lectureEdited);

            //var searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + id);

            //if (Directory.Exists(searchedPath) && oldTitle.Equals(lectureModel.Title) == false)
            //{
            //    Directory.Delete(searchedPath, true);
            //}

            if (lectureModel.File != null)
            {
                foreach (var file in lectureModel.File)
                {
                    if (file.Length > 0)
                    {
                        var path = Path.Combine(_env.WebRootPath, "Lectures/" + id);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        // string extension = lectureModel.Title + "." + Path.GetExtension(file.FileName).Substring(1);

                        using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }

        public void DeleteLecture(Lecture lecture)
        {
            _repository.DeleteLecture(lecture);
        }

        public void DeleFromPath(Guid id)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + id);

            if (Directory.Exists(searchedPath))
            {
                Directory.Delete(searchedPath, true);
            }
        }

        public Stream SearchLecture(Guid lectureId, string fileName)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Lectures/" + lectureId + "/" + fileName);
            Stream file = new FileStream(searchedPath, FileMode.Open);

            return file;
        }
    }
}

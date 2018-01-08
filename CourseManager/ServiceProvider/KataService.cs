using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using Business.ServicesInterfaces;

namespace ServicesProvider
{
    public class KataService : IKataService
    {
        private readonly IKataRepository _repository;
        private readonly IHostingEnvironment _env;

        public KataService(IKataRepository repository, IHostingEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public List<string> GetFiles(Guid id)
        {
            var currentLecture = _repository.GetKataById(id);
            var fileList = new List<string>();
            var path = Directory.GetCurrentDirectory() + "\\wwwroot\\Katas\\" + id;

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
            var currentKata = _repository.GetKataInfoByDetails(title, description);
            var fileList = new List<string>();
            var path = Directory.GetCurrentDirectory() + "\\wwwroot\\Katas\\" + currentKata.Id;

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
            var kata = _repository.GetKataById(id);

            var searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + kata.Id);
            if (Directory.Exists(searchedPath))
            {
                Directory.Delete(searchedPath, true);
            }

            _repository.DeleteKata(kata);
        }

        public void DeleteSpecificFiles(string fileName, Guid? givenId)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + givenId.Value + "/" + fileName);
            if (File.Exists(searchedPath))
            {
                File.Delete(searchedPath);
            }
        }
    }
}

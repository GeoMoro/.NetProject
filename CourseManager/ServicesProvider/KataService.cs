using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace ServicesProvider
{
    public class KataService : IKataService
    {
        private readonly IKataRepository _repository;

        public KataService(IKataRepository repository)
        {
            _repository = repository;
        }

        public List<string> GetFiles(Guid id)
        {
            var currentLecture = _repository.GetKataById(id);
            var fileList = new List<string>();
            string path = Directory.GetCurrentDirectory() + "\\wwwroot\\Katas\\" + id;

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
            string path = Directory.GetCurrentDirectory() + "\\wwwroot\\Katas\\" + currentKata.Id;

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

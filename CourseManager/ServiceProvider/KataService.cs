using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Business.ServicesInterfaces;
using System.Threading.Tasks;
using Business.ServicesInterfaces.Models.KataViewModels;
using Data.Domain.Entities;

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

        public async Task CreateKata(KataCreateModel kataCreateModel)
        {
            _repository.CreateKata(
                Kata.CreateKata(
                    kataCreateModel.Title,
                    kataCreateModel.Description
                )
            );

            var currentKata = GetKataInfoByDetails(kataCreateModel.Title, kataCreateModel.Description);

            if (kataCreateModel.File != null)
            {
                foreach (var file in kataCreateModel.File)
                {
                    if (file.Length > 0)
                    {
                        var path = Path.Combine(_env.WebRootPath, "Katas/" + currentKata.Id);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }

        public Kata GetKataInfoByDetails(string title, string description)
        {
            return _repository.GetKataInfoByDetails(title, description);
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
        
        public void DeleteFile(string fileName, Guid? givenId)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + givenId.Value + "/" + fileName);

            if (File.Exists(searchedPath))
            {
                File.Delete(searchedPath);
            }
        }

        public Kata GetKataById(Guid idValue)
        {
            return _repository.GetKataById(idValue);
        }

        public IReadOnlyList<Kata> GetAllKatas()
        {
            return _repository.GetAllKatas();
        }

        public bool GetAll(Guid id)
        {
            return _repository.GetAllKatas().Any(e => e.Id == id);
        }

        public async Task Edit(Guid id, Kata kataEdited, KataEditModel kataModel)
        {
            _repository.EditKata(kataEdited);

            if (kataModel.File != null)
            {

                foreach (var file in kataModel.File)
                {
                    if (file.Length > 0)
                    {
                        var path = Path.Combine(_env.WebRootPath, "Katas/" + id);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }

        public void DeleteKata(Kata kata)
        {
            _repository.DeleteKata(kata);
        }

        public void DeleFromPath(Guid id)
        {
            string searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + id);
            if (Directory.Exists(searchedPath))
            {
                Directory.Delete(searchedPath, true);
            }
        }

        public Stream SearchKata(Guid kataId, string fileName)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Katas/" + kataId + "/" + fileName);
            Stream file = new FileStream(searchedPath, FileMode.Open);

            return file;
        }
    }

}

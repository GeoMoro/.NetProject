using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using Business.ServicesInterfaces;
using System.Threading.Tasks;
using Business.ServicesInterfaces.Models.UploadsViewModels;
using Data.Persistance;

namespace ServicesProvider
{
    public class UploadService : IUploadService
    {
        private readonly IHostingEnvironment _env;
        private readonly ApplicationDbContext _app;

        public UploadService(IHostingEnvironment env, ApplicationDbContext app)
        {
            _env = env;
            _app = app;
        }

        public Stream DownloadFile(string seminarName, string group, string seminarNumber, string fileName)
        {
            string searchedPath = Path.Combine(_env.WebRootPath, "Uploads/" + seminarName + "/" + seminarNumber + "/" + fileName);

            Stream file = new FileStream(searchedPath, FileMode.Open);

            return file;
        }

        public async Task CreateUploads(string userGroup, string userFirstName, string userLastName, UploadsCreateModel uploadCreateModel)
        {
            //long size = uploadCreateModel.File;

            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            var file = uploadCreateModel.File;
            if (file.Length > 0)
            {
                string path = Path.Combine(_env.WebRootPath, "Uploads/" + uploadCreateModel.Type + "//" + uploadCreateModel.Seminar);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                string extension = userFirstName + "" + userLastName + "" + userGroup + "." + Path.GetExtension(file.FileName).Substring(1);
                using (var fileStream = new FileStream(Path.Combine(path, extension), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }

        public List<string> GetFiles(string type, string seminar, string name)
        {
            List<string> fileList = new List<string>();
            string path = Path.Combine(_env.WebRootPath, "Uploads/" + type + "//" + seminar);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            foreach (var files in Directory.GetFiles(path))
            {
                if(Path.GetFileNameWithoutExtension(files).Equals(name)) fileList.Add(Path.GetFileName(files));
            }

            return fileList;
        }

        public List<string> GetAllFiles(string type, string seminar)
        {
            List<string> fileList = new List<string>();
            string path = Path.Combine(_env.WebRootPath, "Uploads/" + type + "//" + seminar);

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

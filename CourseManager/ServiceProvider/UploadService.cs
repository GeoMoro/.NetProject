using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using Business.ServicesInterfaces;
using System.Threading.Tasks;
using Business.ServicesInterfaces.Models.UploadsViewModels;

namespace ServicesProvider
{
    public class UploadService : IUploadService
    {
        private readonly IHostingEnvironment _env;

        public UploadService(IHostingEnvironment env)
        {
            _env = env;
        }

        public Stream DownloadFile(string seminarName, string group, string week, string fileName, string teacher)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Uploads/" + teacher + "/" + seminarName + "/" + week + "/" + fileName);
            
            Stream file = new FileStream(searchedPath, FileMode.Open);

            return file;
        }

        public async Task CreateUploads(string userGroup, string userFirstName, string userLastName, UploadsCreateModel uploadCreateModel)
        {
            var file = uploadCreateModel.File;
            var path = Path.Combine(_env.WebRootPath, "Uploads\\" + uploadCreateModel.Teacher + "\\" + uploadCreateModel.Type + "\\" + uploadCreateModel.Week);
            
            var extensions = new List<string>
            {
                ".zip",
                ".rar",
                ".7z"
            };

            var types = new List<string>
            {
                "Laboratory",
                "Kata"
            };

            if (!extensions.Contains(Path.GetExtension(file.FileName)) || !types.Contains(uploadCreateModel.Type)) return;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var extension = userFirstName + "" + userLastName + "" + userGroup + "." +
                               Path.GetExtension(file.FileName).Substring(1);
            using (var fileStream = new FileStream(Path.Combine(path, extension), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        public List<string> GetFiles(string type, string week, string name, string teacher)
        {
            var fileList = new List<string>();
            var path = Path.Combine(_env.WebRootPath, "Uploads\\" + teacher +"\\" + type + "\\" + week);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            foreach (var files in Directory.GetFiles(path))
            {
                if(Path.GetFileNameWithoutExtension(files).Equals(name))
                    fileList.Add(Path.GetFileName(files));
            }

            return fileList;
        }

        public List<string> GetAllFiles(string type, string week, string teacher)
        {
            var fileList = new List<string>();
            var path = Path.Combine(_env.WebRootPath, "Uploads\\" + teacher + "\\" + type + "\\" + week);

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

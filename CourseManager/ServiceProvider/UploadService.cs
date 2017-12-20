using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Data.Domain.Entities.Services;
using Microsoft.AspNetCore.Hosting;

namespace ServiceProvider
{
    public class UploadService : IUploadService
    {
        private readonly IHostingEnvironment _env;
        public UploadService(IHostingEnvironment env)
        {
            _env = env;
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

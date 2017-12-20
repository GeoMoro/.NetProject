using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Presentation.Models.UploadsViewModels
{
    public class UploadsEditModel
    {
        public UploadsEditModel()
        {
        }

        [Required(ErrorMessage = "Please enter the seminar week number")]
        public int Seminar { get; set; }

        [Required(ErrorMessage = "Is this a Homework or Kata assignment?")]
        public string Type { get; set; }

        [Required]
        public IFormFile File { get; set; }
        /*
                public List<string> GetFiles()
                {
                    List<string> fileList = new List<string>();
                    string path = Directory.GetCurrentDirectory() + "\\wwwroot\\Katas\\" + Title;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    foreach (var files in Directory.GetFiles(path))
                    {
                        fileList.Add(Path.GetFileName(files));
                    }

                    return fileList;
                }*/

        public UploadsEditModel(int seminar, string type)
        {
            Seminar = seminar;
            Type = type;
        }
    }
}

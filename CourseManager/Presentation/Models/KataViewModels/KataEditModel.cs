using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Presentation.Models.KataViewModels
{
    public class KataEditModel
    {
        public KataEditModel()
        {
        }

        [Required(ErrorMessage = "A title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A description is required.")]
        [StringLength(2000, ErrorMessage = "Maximum number of characters is 2000!")]
        public string Description { get; set; }

        [Required]
        public IEnumerable<IFormFile> File { get; set; }

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
        }

        public KataEditModel(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}

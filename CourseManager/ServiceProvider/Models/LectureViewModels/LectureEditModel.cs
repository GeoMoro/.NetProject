using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Presentation.Models.LectureViewModels
{
    public class LectureEditModel
    {
        public LectureEditModel()
        {
        }

        [Required(ErrorMessage = "A title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A description is required.")]
        [StringLength(2000, ErrorMessage = "Maximum number of characters is 2000!")]
        public string Description { get; set; }
        
        public IEnumerable<IFormFile> File { get; set; }

        public LectureEditModel(string title, string description)
        {
            Title = title;
            Description = description;
        }

    }
}

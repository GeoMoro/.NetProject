using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.ServicesInterfaces.Models.KataViewModels
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

        public IEnumerable<IFormFile> File { get; set; }

        public KataEditModel(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}

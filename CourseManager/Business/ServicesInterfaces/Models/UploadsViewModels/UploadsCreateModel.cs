using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.ServicesInterfaces.Models.UploadsViewModels
{
    public class UploadsCreateModel
    {
        [Required(ErrorMessage = "Please enter the seminar week number")]
        public string Seminar { get; set; }

        [Required(ErrorMessage = "Is this a Homework or Kata assignment?")]
        public string Type { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}

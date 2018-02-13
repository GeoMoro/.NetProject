using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.ServicesInterfaces.Models.UploadsViewModels
{
    public class UploadsCreateModel
    {
        [Required(ErrorMessage = "Please enter the seminar week number")]
        public string Week { get; set; }

        [Required(ErrorMessage = "Is this a Homework or Kata assignment?")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Who's your teacher?")]
        public string Teacher { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}

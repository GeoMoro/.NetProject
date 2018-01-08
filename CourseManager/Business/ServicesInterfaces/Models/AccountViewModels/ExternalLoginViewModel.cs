using System.ComponentModel.DataAnnotations;

namespace Business.ServicesInterfaces.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

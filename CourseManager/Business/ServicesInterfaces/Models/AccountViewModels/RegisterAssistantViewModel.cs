using System.ComponentModel.DataAnnotations;

namespace Business.ServicesInterfaces.Models.AccountViewModels
{
    public class RegisterAssistantViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [RegularExpression(@"[A-Z][A-Za-z -]*", ErrorMessage = "Only letters, spaces and \"-\". First letters must be capital.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [RegularExpression(@"[A-Z][A-Za-z -]*", ErrorMessage = "Only letters, spaces and \"-\". First letters must be capital.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [MaxLength(20)]
        [Display(Name = "Secret word")]
        public string SecretWord { get; set; }
    }
}

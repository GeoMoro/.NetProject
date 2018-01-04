using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.AccountViewModels
{
    public class RegisterStudentViewModel
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
        [RegularExpression(@"([A - Z]|[0-9]){16}", ErrorMessage = "Registration number consists of 16 digits and capital letters.")]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Required]
        [RegularExpression(@"([AB][1-9])|X", ErrorMessage = "Example: B3/A1 or X")]
        public string Group { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class UserAccountStudentCreateModel
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "RegistrationNumber is required.")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Group is required.")]
        public string Group { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "E-mail address is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
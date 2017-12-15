using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.UsersViewModels
{
    public class UserAccountAssistantCreateModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [MinLength(1, ErrorMessage = "First name must have at least 1 character.")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(1, ErrorMessage = "Last name must have at least 1 character.")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 charaters long.")]
        [MaxLength(100, ErrorMessage = "Password must not exceed 100 charaters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "E-mail address is required.")]
        [MaxLength(200, ErrorMessage = "E-mail adress must not exceed 200 charaters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
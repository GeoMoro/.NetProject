using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class UserAccountAdminEditModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [MinLength(1, ErrorMessage = "First name must have at least 1 character.")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(1, ErrorMessage = "Last name must have at least 1 character.")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-mail address is required.")]
        [MaxLength(200, ErrorMessage = "E-mail adress must not exceed 200 charaters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public UserAccountAdminEditModel(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
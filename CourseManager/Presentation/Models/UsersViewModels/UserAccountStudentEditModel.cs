using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.UsersViewModels
{
    public class UserAccountStudentEditModel
    {
        public UserAccountStudentEditModel()
        {
            // EF
        }

        [Required(ErrorMessage = "First name is required.")]
        [MinLength(1, ErrorMessage = "First name must have at least 1 character.")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(1, ErrorMessage = "Last name must have at least 1 character.")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [StringLength(16, ErrorMessage = "Registration number must have 16 characters.", MinimumLength = 16)]
        public string RegistrationNumber { get; set; }

        [RegularExpression(@"([AB][1-9])|X")]
        public string Group { get; set; }

        [Required(ErrorMessage = "E-mail address is required.")]
        [MaxLength(200, ErrorMessage = "E-mail adress must not exceed 200 charaters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public UserAccountStudentEditModel(string firstName, string lastName, string registrationNumber, string group, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            RegistrationNumber = registrationNumber;
            Group = group;
            Email = email;
        }
    }
}
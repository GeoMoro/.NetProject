using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class UserAccount
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MinLength(1, ErrorMessage = "First name must have at least 1 character.")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(1, ErrorMessage = "Last name must have at least 1 character.")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [StringLength(16, ErrorMessage = "Registration number must have 16 characters.")]
        public string RegistrationNumber { get; set; }

        [RegularExpression(@"([AB][1-9])|X")]
        public string Group { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 charaters long.")]
        [MaxLength(100, ErrorMessage = "Password must not exceed 100 charaters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "E-mail address is required.")]
        [MaxLength(200, ErrorMessage = "E-mail adress must not exceed 200 charaters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public int Rank { get; set; }

        public bool Validated { get; set; }

        public static UserAccount CreateStudentAccount(string firstName, string lastName, string registrationNumber,
            string group, string password, string email)
        {
            var instance = new UserAccount
            {
                Id = Guid.NewGuid(),
                Rank = 2,
                Validated = false
            };
            instance.UpdateStudent(firstName, lastName, registrationNumber, group, password, email);

            return instance;
        }

        public static UserAccount CreateAssistantAccount(string firstName, string lastName, string password,
            string email)
        {
            var instance = new UserAccount
            {
                Id = Guid.NewGuid(),
                Rank = 1,
                Validated = false
            };
            instance.UpdateAssistant(firstName, lastName, password, email);

            return instance;
        }

        private void UpdateStudent(string firstName, string lastName, string registrationNumber, string group, string password, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            RegistrationNumber = registrationNumber;
            Group = group;
            Password = password;
            Email = email;
        }

        private void UpdateAssistant(string firstName, string lastName, string password, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Email = email;
        }
    }
}
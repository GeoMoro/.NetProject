using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class UserAccount
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        public string RegistrationNumber { get; set; }

        public string Group { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "E-mail address is required.")]
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
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class UserAccountStudentEditModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string RegistrationNumber { get; set; }

        public string Group { get; set; }

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
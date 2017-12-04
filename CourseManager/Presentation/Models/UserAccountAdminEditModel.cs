using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class UserAccountAdminEditModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

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
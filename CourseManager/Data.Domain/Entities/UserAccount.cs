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
    }
}
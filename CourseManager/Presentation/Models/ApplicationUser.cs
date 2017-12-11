using System;
using Microsoft.AspNetCore.Identity;

namespace Presentation.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string RegistrationNumber { get; set; }

        public string Group { get; set; }
    }
}

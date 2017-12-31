using System.Collections.Generic;
using Data.Domain.Entities;

namespace Presentation.Models.UserManagementViewModels
{
    public class UserManagementIndexViewModel
    {
        public List<ApplicationUser> Users { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Business.ServicesInterfaces.Models.UserManagementViewModels
{
    public class UserManagementAddRoleViewModel
    {
        public string UserId { get; set; }

        public string NewRole { get; set; }

        public SelectList Roles { get; set; }

        public string Fullname { get; set; }
    }
}
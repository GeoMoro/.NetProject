using Microsoft.AspNetCore.Identity;

namespace Presentation.Configuration
{
    public class UserRoleSeed
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleSeed(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public void Seed()
        {
            CreateRole("Owner");
            CreateRole("Assistant");
            CreateRole("Student");
        }
        private async void CreateRole(string role)
        {
            if ((await _roleManager.FindByNameAsync(role)) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = role });
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Presentation.Configuration
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData(RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            roleManager.CreateRole("Owner");
            roleManager.CreateRole("Assistant");
            roleManager.CreateRole("Student");
        }

        public static void CreateRole(this RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!roleManager.RoleExistsAsync(roleName).Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = roleName
                };

                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}

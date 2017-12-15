using Microsoft.AspNetCore.Identity;

namespace Presentation.Data
{
    public static class ApplicationDbContextExtensions
    {
        public static async void Seed(this ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole { Name = "Owner" });
            await roleManager.CreateAsync(new IdentityRole { Name = "Assistant" });
            await roleManager.CreateAsync(new IdentityRole { Name = "Student" });
        }
    }
}

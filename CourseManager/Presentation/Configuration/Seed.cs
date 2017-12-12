using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Data;
using Presentation.Models;

namespace Presentation.Configuration
{
    public static class Seed
    {
        public static void Initialize(IServiceProvider provider)
        {
            var _context = provider.GetRequiredService<ApplicationDbContext>();
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        }
    }
}

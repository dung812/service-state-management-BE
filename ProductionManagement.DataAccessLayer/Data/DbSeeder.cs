using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System;
using ProductionManagement.Models;
using ProductionManagement.DataContract.Enum;
using ProductionManagement.DataContract.Constant;

namespace ProductionManagement.DataAccessLayer.Data
{
    public class DbSeeder
    {
        public static async Task SeedUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, string mailDomain)
        {
            if (!userManager.Users.Any())
            {
                //Seed Roles
                await roleManager.CreateAsync(new IdentityRole(ERoles.Admin.ToString()));

                // Seed default admin
                var admin = new User
                {
                    UserName = "admin",
                    Email = $"admin{mailDomain}",
                    IsDisabled = false,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Name = "admin",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                };
                var adminInDb = await userManager.FindByEmailAsync(admin.Email);
                if (adminInDb == null)
                {
                    await userManager.CreateAsync(admin, DefaultPassword.Admin);
                    await userManager.AddToRoleAsync(admin, ERoles.Admin.ToString());
                }
            }
        }
    }
}

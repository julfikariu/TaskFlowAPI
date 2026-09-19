using Microsoft.AspNetCore.Identity;
using TaskFlowAPI.Model;

namespace TaskFlowAPI.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
            {
                "Admin",
                "Manager",
                "Member"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role)
                    );
                }
            }
        }

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "admin@example.com";
            const string adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(
                adminEmail
            );

            if (adminUser is null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator"
                };

                var result = await userManager.CreateAsync(
                    adminUser,
                    adminPassword
                );

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(
                            ", ",
                            result.Errors.Select(error => error.Description)
                        )
                    );
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(
                    adminUser,
                    "Admin"
                );
            }
        }



    }
}

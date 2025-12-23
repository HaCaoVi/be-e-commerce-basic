using e_commerce_basic.Models;
using Microsoft.AspNetCore.Identity;

namespace e_commerce_basic.Database.Seedings
{
    public class IdentitySeed
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task SeedAdminAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var config = services.GetRequiredService<IConfiguration>();
            var email = config["AdminAccount:Email"];
            var password = config["AdminAccount:Password"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return;

            var admin = await userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                admin = new User
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    IsActivated = true
                };

                var result = await userManager.CreateAsync(admin, password);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description)
                    ));
                }
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

    }
}
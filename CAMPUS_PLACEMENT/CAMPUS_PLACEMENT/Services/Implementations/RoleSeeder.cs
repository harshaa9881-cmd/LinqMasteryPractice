using Microsoft.AspNetCore.Identity;
using CampusPlacementManagement.Constants;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Services.Implementations
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider services,
            IConfiguration configuration)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles =
            {
                UserRoles.SuperAdmin,
                UserRoles.PlacementOfficer,
                UserRoles.CompanyHR,
                UserRoles.Student
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var email = configuration["SuperAdmin:Email"];

            var password = configuration["SuperAdmin:Password"];

            var fullName = configuration["SuperAdmin:FullName"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var admin = await userManager.FindByEmailAsync(email);

            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName ?? "System Administrator",
                    EmailConfirmed = true,
                    AccountStatus = AccountStatus.Approved,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(
                    admin,
                    password
                );

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join(" ", result.Errors.Select(e => e.Description))
                    );
                }
            }

            if (!await userManager.IsInRoleAsync(
                admin,
                UserRoles.SuperAdmin))
            {
                await userManager.AddToRoleAsync(
                    admin,
                    UserRoles.SuperAdmin
                );
            }
        }
    }
}
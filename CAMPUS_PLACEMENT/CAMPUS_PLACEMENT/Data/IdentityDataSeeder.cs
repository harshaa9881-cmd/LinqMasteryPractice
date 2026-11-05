using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Data
{
    public class IdentityDataSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles =
            {
                "SuperAdmin",
                "PlacementOfficer",
                "CompanyHR",
                "Student"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            string.Join(", ",
                                result.Errors.Select(e => e.Description)));
                    }
                }
            }

            // Initial SuperAdmin Configuration

            var email = configuration["SeedAdmin:Email"];
            var password = configuration["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "SuperAdmin email and password must be configured in User Secrets or environment variables.");
            }

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = "System Administrator",
                    EmailConfirmed = true,
                    AccountStatus = AccountStatus.Approved
                };

                var result = await userManager.CreateAsync(admin, password);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ",
                            result.Errors.Select(e => e.Description)));
                }
            }
            else if (admin.AccountStatus != AccountStatus.Approved)
            {
                admin.AccountStatus = AccountStatus.Approved;

                var updateResult = await userManager.UpdateAsync(admin);

                if (!updateResult.Succeeded)
                {
                    throw new Exception(
                        "Unable to update SuperAdmin account.");
                }
            }

            if (!await userManager.IsInRoleAsync(admin, "SuperAdmin"))
            {
                var result = await userManager.AddToRoleAsync(
                    admin, "SuperAdmin");

                if (!result.Succeeded)
                {
                    throw new Exception(
                        "Unable to assign SuperAdmin role.");
                }
            }
        }
    }
}
using Microsoft.AspNetCore.Identity;
using NordesteFoodAPI.Modules.Auth.Domain.Enums;

namespace NordesteFoodAPI.Shared.Infraestructure.Identity
{
    public class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            foreach (UserRole role in Enum.GetValues<UserRole>())
            {
                string roleName = role.ToString().ToUpper();

                bool roleExists = await roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
        }
    }
}

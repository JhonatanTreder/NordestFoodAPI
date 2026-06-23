using Microsoft.AspNetCore.Identity;

namespace NordesteFoodAPI.Shared.Infraestructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}

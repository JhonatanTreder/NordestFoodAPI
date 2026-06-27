using NordesteFoodAPI.Modules.Restaurants.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.Restaurants.Domain.Entities
{
    public class Restaurant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Address Address { get; set; } = null!;
        public ComercialName ComercialName { get; set; } = null!;
        public OperationTime OperationTime { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}

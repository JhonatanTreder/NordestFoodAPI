namespace NordesteFoodAPI.Modules.Restaurants.Domain.DTOs
{
    public class CreateRestaurantRequestDTO
    {
        public string Address { get; set; } = null!;
        public string ComercialName { get; set; } = null!;
        public string OpeningTime { get; set; } = null!;
        public string ClosingTime { get; set; } = null!;
    }
}

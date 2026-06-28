using Microsoft.EntityFrameworkCore;
using NordesteFoodAPI.Modules.Restaurants.Domain.Contracts;
using NordesteFoodAPI.Modules.Restaurants.Domain.DTOs;
using NordesteFoodAPI.Modules.Restaurants.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Infraestructure.Persistence;

namespace NordesteFoodAPI.Modules.Restaurants.Infraestructure.Persistence.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly AppDbContext _dbContext;

        public RestaurantRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<CreateRestaurantResponseDTO>> CreateRestaurantAsync(Restaurant newRestaurant)
        {
            await _dbContext.Restaurants.AddAsync(newRestaurant);
            await _dbContext.SaveChangesAsync();

            var createResponse = new CreateRestaurantResponseDTO(
                newRestaurant.Id,
                newRestaurant.Address.Value,
                newRestaurant.OperationTime.OpeningTime.ToString(),
                newRestaurant.OperationTime.ClosingTime.ToString(),
                newRestaurant.IsActive,
                newRestaurant.CreatedAt
            );

            return Result<CreateRestaurantResponseDTO>.Success(createResponse);
        }

        public async Task<Restaurant?> FindByIdAsync(Guid restaurantId)
        {
            return await _dbContext.Restaurants.FindAsync(restaurantId);
        }

        public async Task<Restaurant?> FindByNameAsync(string restaurantName)
        {
            return await _dbContext.Restaurants
                .FirstOrDefaultAsync(r => r.ComercialName.Value == restaurantName);
        }

        public async Task<IEnumerable<Restaurant?>> FindRestaurantsAsync()
        {
            return await _dbContext.Restaurants
                .Where(r => r.IsActive)
                .ToListAsync();
        }
    }
}

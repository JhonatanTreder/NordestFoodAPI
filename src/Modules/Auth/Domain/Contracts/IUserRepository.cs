using NordesteFoodAPI.Modules.Auth.Domain.Entities;

namespace NordesteFoodAPI.Modules.Auth.Domain.Contracts
{
    public interface IUserRepository
    {
        Task<bool> ExistsUserByEmailAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task SaveAsync(User user);
    }
}

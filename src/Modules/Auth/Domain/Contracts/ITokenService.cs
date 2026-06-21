using NordesteFoodAPI.Modules.Auth.Domain.Enums;

namespace NordesteFoodAPI.Modules.Auth.Domain.Contracts
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string username, UserRole userRole);
    }
}

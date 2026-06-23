namespace NordesteFoodAPI.Modules.Auth.Domain.Contracts
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string username, string userRole);
    }
}

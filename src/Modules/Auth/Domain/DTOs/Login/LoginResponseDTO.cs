using NordesteFoodAPI.Modules.Auth.Domain.Enums;

namespace NordesteFoodAPI.Modules.Auth.Domain.DTOs.Login
{
    public record LoginResponseDTO(string Token, Guid UserId, string Username, UserRole UserRole);
}

namespace NordesteFoodAPI.Modules.Auth.Domain.DTOs.Register
{
    public record RegisterUserRequestDTO(string Username, string Email, string? PhoneNumber, string Password);
}

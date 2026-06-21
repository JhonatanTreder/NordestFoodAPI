using NordesteFoodAPI.Modules.Auth.Application.Exceptions;
using NordesteFoodAPI.Modules.Auth.Domain.Contracts;
using NordesteFoodAPI.Modules.Auth.Domain.DTOs.Login;

namespace NordesteFoodAPI.Modules.Auth.Application.UseCases
{
    public class LoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public LoginUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userRepository.GetByEmailAsync(loginRequestDTO.Email);

            if (user is null || !_passwordHasher.Verify(loginRequestDTO.Password, user.PasswordHash))
                throw new ApplicationLayerException("Email ou senha inválidos.");

            var token = _tokenService.GenerateAccessToken(user.Id, user.Username.Value, user.UserRole);

            return new LoginResponseDTO(
                token,
                user.Id,
                user.Username.Value,
                user.UserRole
            );
        }
    }
}

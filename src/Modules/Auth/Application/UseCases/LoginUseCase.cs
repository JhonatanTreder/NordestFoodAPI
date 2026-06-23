using Microsoft.AspNetCore.Identity;
using NordesteFoodAPI.Modules.Auth.Application.Exceptions;
using NordesteFoodAPI.Modules.Auth.Domain.Contracts;
using NordesteFoodAPI.Modules.Auth.Domain.DTOs.Login;
using NordesteFoodAPI.Shared.Infraestructure.Identity;

namespace NordesteFoodAPI.Modules.Auth.Application.UseCases
{
    public class LoginUseCase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginUseCase(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
                throw new ApplicationLayerException("Email ou senha inválidos.");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Client";

            var token = _tokenService.GenerateAccessToken(user.Id, user.UserName!, role);

            return new LoginResponseDTO(token, user.Id, user.UserName!, role);
        }
    }
}
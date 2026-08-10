using Microsoft.AspNetCore.Identity;
using NordesteFoodAPI.Modules.Auth.Application.Exceptions;
using NordesteFoodAPI.Modules.Auth.Domain.Contracts;
using NordesteFoodAPI.Modules.Auth.Domain.DTOs.Login;
using NordesteFoodAPI.Shared.Common.Results;
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

        public async Task<Result<LoginResponseDTO>> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
            {
                return Result<LoginResponseDTO>.Failure(
                    "Não foi possível fazer o login: Email ou senha inválidos.",
                    ErrorType.Unauthorized
                );
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Client";

            var token = _tokenService.GenerateAccessToken(user.Id, user.UserName!, role);

            var loginResponseDTO = new LoginResponseDTO(token, user.Id, user.UserName!, role);

            return Result<LoginResponseDTO>.Success(loginResponseDTO);
        }
    }
}
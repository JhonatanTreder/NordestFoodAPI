using Microsoft.AspNetCore.Identity;
using NordesteFoodAPI.Modules.Auth.Application.Exceptions;
using NordesteFoodAPI.Modules.Auth.Domain.DTOs.Register;
using NordesteFoodAPI.Modules.Auth.Domain.Enums;
using NordesteFoodAPI.Shared.Infraestructure.Identity;

namespace NordesteFoodAPI.Modules.Auth.Application.UseCases
{
    public class RegisterUseCase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUseCase(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Guid> Register(RegisterUserRequestDTO registerUserDTO)
        {
            var newUser = new ApplicationUser
            {
                UserName = registerUserDTO.Username,
                Email = registerUserDTO.Email,
                PhoneNumber = registerUserDTO.PhoneNumber,
                Name = registerUserDTO.Username,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(newUser, registerUserDTO.Password);

            if (!result.Succeeded)
                throw new ApplicationLayerException(string.Join("; ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(newUser, UserRole.Client.ToString().ToUpper());

            return newUser.Id;
        }
    }
}
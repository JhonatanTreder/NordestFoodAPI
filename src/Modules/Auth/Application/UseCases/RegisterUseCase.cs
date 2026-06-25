using Microsoft.AspNetCore.Identity;
using NordesteFoodAPI.Modules.Auth.Application.Exceptions;
using NordesteFoodAPI.Modules.Auth.Domain.DTOs.Register;
using NordesteFoodAPI.Modules.Auth.Domain.Enums;
using NordesteFoodAPI.Shared.Infraestructure.Identity;
using NordesteFoodAPI.Shared.Infraestructure.Results;

namespace NordesteFoodAPI.Modules.Auth.Application.UseCases
{
    public class RegisterUseCase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUseCase(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<Guid>> Register(RegisterUserRequestDTO registerUserDTO)
        {
            var newUser = new ApplicationUser
            {
                UserName = registerUserDTO.Username,
                Email = registerUserDTO.Email,
                PhoneNumber = registerUserDTO.PhoneNumber,
                Name = registerUserDTO.Username,
                CreatedAt = DateTime.UtcNow
            };

            var existingUserByEmail = await _userManager.FindByEmailAsync(registerUserDTO.Email);

            if (existingUserByEmail is not null)
            {
                return Result<Guid>.Failure($"Erro ao criar o usuário: O E-mail {registerUserDTO.Email} já existe.", ErrorType.Conflict);
            }

            var createResult = await _userManager.CreateAsync(newUser, registerUserDTO.Password);

            if (!createResult.Succeeded)
            {
                var errorMessage = string.Join("; ", createResult.Errors.Select(e => e.Description));
                return Result<Guid>.Failure(errorMessage, ErrorType.CreateConflict);
            }

            var assignRoleResult = await _userManager.AddToRoleAsync(newUser, UserRole.Client.ToString().ToUpper());

            if (!assignRoleResult.Succeeded)
            {
                var errorMessage = string.Join("; ", assignRoleResult.Errors.Select(e => e.Description));
                return Result<Guid>.Failure($"O usuário foi criado, mas houve uma falha ao atribuir à role: {errorMessage}", ErrorType.UnexpectedFailure);
            }

            return Result<Guid>.Success(newUser.Id);
        }
    }
}
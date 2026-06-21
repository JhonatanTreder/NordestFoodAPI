using NordesteFoodAPI.Modules.Auth.Domain.Contracts;
using NordesteFoodAPI.Modules.Auth.Domain.DTOs.Register;
using NordesteFoodAPI.Modules.Auth.Domain.Entities;
using NordesteFoodAPI.Modules.Auth.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.Auth.Application.UseCases
{
    public class RegisterUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Register(RegisterUserRequestDTO registerUserDTO)
        {
            var email = Email.Create(registerUserDTO.Email);
            var username = Username.Create(registerUserDTO.Username);
            var password = Password.Create(registerUserDTO.Password);
            var phoneNumber = PhoneNumber.Create(registerUserDTO.PhoneNumber);

            var existingUser = await _userRepository.ExistsUserByEmailAsync(email.Value);

            if (existingUser) 
                throw new ApplicationException("O usuário com este Email já existe.");

            var passwordHash = _passwordHasher.Hash(password.Value);

            var newUser = User.Register(username, email, password, phoneNumber);

            await _userRepository.SaveAsync(newUser);

            return newUser.Id;
        }
    }
}

using NordesteFoodAPI.Modules.Auth.Domain.Enums;
using NordesteFoodAPI.Modules.Auth.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.Auth.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }

        public UserRole UserRole { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Email Email { get; private set; } = null!;
        public Username Username { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public PhoneNumber? PhoneNumber { get; private set; } = null!;

        private User() { }

        public static User Register(Username username, Email email, string passwordHash, PhoneNumber? phoneNumber)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UserRole = UserRole.Client //OBS: Todo usuário registrado é criado como 'Client', a atualização do papel é atribuído posteriormente pelo 'Admin'.
            };
        }
    }
}

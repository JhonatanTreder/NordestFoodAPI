using NordesteFoodAPI.Shared.Domain.Utils;
using System.Text.RegularExpressions;

namespace NordesteFoodAPI.Modules.Auth.Domain.ValueObjects
{
    public class Password
    {
        public string Value { get; } = string.Empty;

        private Password(string value) => Value = value;

        public static Password Create(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || !Regex.IsMatch(password, RegexPatterns.PasswordRegex))
                throw new Exception("Invalid password format.");

            return new Password(password);
        }
    }
}

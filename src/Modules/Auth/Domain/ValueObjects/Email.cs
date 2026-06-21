using NordesteFoodAPI.Shared.Domain.Utils;
using System.Text.RegularExpressions;

namespace NordesteFoodAPI.Modules.Auth.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; } = string.Empty;

        private Email(string value) => Value = value;

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, RegexPatterns.EmailRegex))
                throw new Exception("Invalid email format.");

            return new Email(email);
        }
    }
}

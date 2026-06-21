using NordesteFoodAPI.Shared.Domain.Utils;
using System.Text.RegularExpressions;

namespace NordesteFoodAPI.Modules.Auth.Domain.ValueObjects
{
    public class Username
    {
        public string Value { get; } = string.Empty;

        private Username(string value) => Value = value;

        public static Username Create(string username)
        {
            if (string.IsNullOrWhiteSpace(username) || !Regex.IsMatch(username, RegexPatterns.UsernameRegex))
                throw new Exception("Invalid username.");

            return new Username(username);
        }
    }
}

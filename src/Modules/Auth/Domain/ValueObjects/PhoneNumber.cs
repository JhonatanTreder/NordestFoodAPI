using NordesteFoodAPI.Shared.Domain.Utils;
using System.Text.RegularExpressions;

namespace NordesteFoodAPI.Modules.Auth.Domain.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; } = string.Empty;

        private PhoneNumber(string value) => Value = value;

        public static PhoneNumber Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, RegexPatterns.PhoneNumberRegex))
                throw new Exception("Invalid PhoneNumber format.");

            return new PhoneNumber(phoneNumber);
        }
    }
}

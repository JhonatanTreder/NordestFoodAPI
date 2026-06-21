using NordesteFoodAPI.Modules.Auth.Domain.Exceptions;
using NordesteFoodAPI.Shared.Domain.Utils;
using System.Text.RegularExpressions;

namespace NordesteFoodAPI.Modules.Auth.Domain.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; } = string.Empty;

        private PhoneNumber(string value) => Value = value;

        public static PhoneNumber? Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (!Regex.IsMatch(value, @"^[\d+\-]+$"))
                throw new DomainException("Telefone deve conter apenas números, + e -.");

            var digitsOnly = Regex.Replace(value, @"[^\d]", "");

            if (digitsOnly.Length < 8 || digitsOnly.Length > 15)
                throw new DomainException("Telefone deve ter entre 8 e 15 dígitos.");

            return new PhoneNumber(value);
        }

        public override string ToString() => Value;
    }
}

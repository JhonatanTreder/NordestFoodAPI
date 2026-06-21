using System.Text.RegularExpressions;
namespace NordesteFoodAPI.Shared.Domain.Utils
{
    public class RegexPatterns
    {
        public const string EmailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,64}$";
        public const string UsernameRegex = @"^[a-zA-Z_]{3,55}$";
        public const string PhoneNumberRegex = @"^\+?[0-9]{8,15}$";
    }
}

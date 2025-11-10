using System.Text.RegularExpressions;

namespace ProjectCenter.Core.ValueObjects
{
    public static class PhoneValidator
    {
        public static bool IsValid(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return Regex.IsMatch(phone, @"^(\+7|8)\d{10}$");
        }
    }
}

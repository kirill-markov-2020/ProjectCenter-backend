using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectCenter.Core.ValueObjects
{
    public static class PasswordValidator
    {
        public static List<string> Validate(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(password))
            {
                errors.Add("Пароль не может быть пустым.");
                return errors;
            }

            if (password.Length < 8)
                errors.Add("Пароль должен содержать минимум 8 символов.");

            if (Regex.IsMatch(password, "[а-яА-ЯёЁ]"))
                errors.Add("Пароль должен содержать только латинские буквы (английский алфавит).");

            if (!Regex.IsMatch(password, "[a-z]"))
                errors.Add("Пароль должен содержать хотя бы одну строчную латинскую букву.");

            if (!Regex.IsMatch(password, "[A-Z]"))
                errors.Add("Пароль должен содержать хотя бы одну заглавную латинскую букву.");

            if (!Regex.IsMatch(password, "[0-9]"))
                errors.Add("Пароль должен содержать хотя бы одну цифру.");

            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};:'"",.<>/?\\|]"))
                errors.Add("Пароль должен содержать хотя бы один специальный символ.");

            return errors;
        }
    }
}

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProjectCenter.Core.ValueObjects
{
    public static class EmailValidator
    {
        public static List<string> Validate(string email)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(email))
            {
                errors.Add("Email не может быть пустым.");
                return errors;
            }

            if (Regex.IsMatch(email, "[а-яА-ЯёЁ]"))
                errors.Add("Email должен содержать только латинские символы.");

          
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$"))
                errors.Add("Email имеет некорректный формат. Пример: example@mail.com");

            return errors;
        }
    }
}

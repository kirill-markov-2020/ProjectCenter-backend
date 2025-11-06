using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Core.ValueObjects
{
    public class EmailAddress
    {
        public string Value { get; private set; }

        private EmailAddress() { } 

        public EmailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email не может быть пустым.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Некорректный формат email.");

            Value = value;
        }

        public override string ToString() => Value;
    }
}

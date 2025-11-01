using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Core.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; private set; }
        private PhoneNumber() { }
        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Телефон не может быть пустым.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^\+?\d+$"))
                throw new ArgumentException("Телефон должен содержать только цифры и может начинаться с +.");

            Value = value;
        }

        public override string ToString() => Value;
    }
}

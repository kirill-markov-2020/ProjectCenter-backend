using System;

namespace ProjectCenter.Core.Exceptions
{
    public class InvalidPhoneNumberException : Exception
    {
        public InvalidPhoneNumberException(string message) : base(message)
        {
        }
    }
}

using System;

namespace ProjectCenter.Core.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string message) : base(message) { }
    }
}

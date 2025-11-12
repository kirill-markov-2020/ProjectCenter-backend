using System;

namespace ProjectCenter.Core.Exceptions
{
    public class InvalidUserUpdateException : Exception
    {
        public InvalidUserUpdateException(string message)
            : base(message) { }
    }
}

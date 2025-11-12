using System;

namespace ProjectCenter.Core.Exceptions
{
    public class ForbiddenUserUpdateException : Exception
    {
        public ForbiddenUserUpdateException()
            : base("Вы не можете изменять данные этого пользователя.") { }

        public ForbiddenUserUpdateException(string message) : base(message) { }
    }
}

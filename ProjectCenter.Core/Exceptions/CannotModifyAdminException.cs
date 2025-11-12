using System;

namespace ProjectCenter.Core.Exceptions
{
    public class CannotModifyAdminException : Exception
    {
        public CannotModifyAdminException()
            : base("Нельзя изменять данные администратора.") { }

        public CannotModifyAdminException(string message) : base(message) { }
    }
}

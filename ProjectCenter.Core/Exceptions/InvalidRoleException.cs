namespace ProjectCenter.Core.Exceptions
{
    public class InvalidRoleException : Exception
    {
        public InvalidRoleException(string role)
            : base($"Роль «{role}» недопустима. Разрешённые роли: Student, Teacher, Admin.")
        {
        }
    }
}

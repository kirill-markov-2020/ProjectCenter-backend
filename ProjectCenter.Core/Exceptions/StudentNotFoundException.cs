namespace ProjectCenter.Core.Exceptions
{
    public class StudentNotFoundException : Exception
    {
        public StudentNotFoundException(int userId)
            : base($"Студент с UserId {userId} не найден.") { }
    }
}
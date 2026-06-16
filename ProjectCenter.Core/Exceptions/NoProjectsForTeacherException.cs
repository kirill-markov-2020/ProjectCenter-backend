
namespace ProjectCenter.Core.Exceptions
{
    public class NoProjectsForTeacherException : Exception
    {
        public NoProjectsForTeacherException(int teacherId)
            : base($"У вас нет закреплённых студентов или они ещё не создали проекты.")
        {
        }

        public NoProjectsForTeacherException(string message) : base(message)
        {
        }
    }
}
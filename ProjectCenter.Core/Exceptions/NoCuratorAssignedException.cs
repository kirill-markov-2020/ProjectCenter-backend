namespace ProjectCenter.Core.Exceptions
{
    public class NoCuratorAssignedException : Exception
    {
        public NoCuratorAssignedException(int studentId)
            : base($"Студенту с ID {studentId} не назначен куратор.") { }
    }
}
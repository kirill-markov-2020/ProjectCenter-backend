namespace ProjectCenter.Core.Exceptions
{
    public class TeacherHasStudentsException : Exception
    {
        public TeacherHasStudentsException()
            : base("Невозможно удалить преподавателя, у которого есть закрепленные студенты.") { }

        public TeacherHasStudentsException(string message)
            : base(message) { }
    }
}

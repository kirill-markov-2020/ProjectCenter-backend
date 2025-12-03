namespace ProjectCenter.Core.Exceptions
{
    public class ProjectAccessDeniedException : Exception
    {
        public ProjectAccessDeniedException()
            : base("У вас нет прав для редактирования этого проекта") { }
    }
}
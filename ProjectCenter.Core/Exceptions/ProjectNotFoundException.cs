namespace ProjectCenter.Core.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(int id)
            : base($"Проект с ID {id} не найден.") { }
    }
}
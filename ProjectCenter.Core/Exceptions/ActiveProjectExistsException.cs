namespace ProjectCenter.Core.Exceptions
{
    public class ActiveProjectExistsException : Exception
    {
        public ActiveProjectExistsException(string projectTitle)
            : base($"У вас уже есть активный проект с темой «{projectTitle}». Вы не можете создать больше одного проекта.") { }
    }
}
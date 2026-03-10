namespace ProjectCenter.Core.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int id)
            : base($"Пользователь с ID {id} не найден.") { }
    }
}

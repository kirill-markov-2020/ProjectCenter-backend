namespace ProjectCenter.Core.Exceptions
{
    public class FileValidationException : Exception
    {
        public FileValidationException(string message) : base(message) { }
    }
}
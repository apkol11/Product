namespace Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a business rule is violated.
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}

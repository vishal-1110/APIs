namespace APIs.Exceptions
{
    public class AppExceptions
    {
    }

    public class DuplicateOrderException : Exception
    {
        public DuplicateOrderException(string message) : base(message) { }
    }

    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(string message) : base(message) { }
    }
}

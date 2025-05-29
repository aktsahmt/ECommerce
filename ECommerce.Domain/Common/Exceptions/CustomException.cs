namespace ECommerce.Application.Common.Exceptions
{
    public class CustomException : Exception
    {
        public int StatusCode { get; }
        public string Error { get; }
        public CustomException(string message, string error = "", int statusCode = 0) : base(message)
        {
            Error = error;
            StatusCode = statusCode;
        }
    }
}

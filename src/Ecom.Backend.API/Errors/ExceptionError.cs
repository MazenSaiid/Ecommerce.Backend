namespace Ecom.Backend.API.Errors
{
    public class ExceptionError : CommonResponseError
    {
        public ExceptionError(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; }
    }
}

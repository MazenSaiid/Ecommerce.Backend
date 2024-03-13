
namespace Ecom.Backend.API.Errors
{
    public class CommonResponseError
    {
        public CommonResponseError(int statusCode, string message=null)
        {
            StatusCode = statusCode;
            Message = message ?? DefaultMessageForStatusCode(statusCode);
                
        }

        private string DefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Not Authorized",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}

namespace Ecom.Backend.API.Errors
{
    public class ValidationError : CommonResponseError
    {
        public ValidationError() : base(400)
        {

        }
        public IEnumerable<string> Errors { get; set; }
    }
}

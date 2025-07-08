using System.Net;

namespace WebApiDemo.Model
{
    public class ApiResponse
    {
        public HttpStatusCode statusCode { get; set; }

        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessage { get; set; }
        public object result { get;set; }
    }
}

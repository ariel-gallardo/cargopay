using Infraestructure;
using Microsoft.AspNetCore.Http;

namespace Domain
{
    public class CustomResponse
    {
        public dynamic Data { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class CustomResponse<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}

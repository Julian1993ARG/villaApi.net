using System.Net;

namespace MagicVilla.Models
{
    public class DefaultResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Ok { get; set; } = true;
        public List<string>? ErrorMessage { get; set; }
        public object? Data { get; set; }
    }
}

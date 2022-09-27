
using System.Net;

namespace Lookup.Service.Models
{
    public class LookupResponse
    {
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string State { get; set; }
    }
}

using System.Text.Json;

namespace BPM.Web.API.GlobalExceptionHandling
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        // Overriding ToString() to automatically serialize the object into JSON
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

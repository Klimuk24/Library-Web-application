using System.Text.Json;

namespace Library_Web_application.Infrastructure.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? StackTrace { get; set; } 

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
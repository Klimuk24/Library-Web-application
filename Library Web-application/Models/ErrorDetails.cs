using System.Text.Json;

namespace Library_Web_application.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string? StackTrace { get; set; } // Опционально для разработки

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
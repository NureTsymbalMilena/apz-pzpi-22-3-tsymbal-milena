using System.ComponentModel.DataAnnotations;

namespace InRoom.DLL.Models;

public class RequestLog
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Method { get; set; } = default!;
    public string Path { get; set; } = default!;
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
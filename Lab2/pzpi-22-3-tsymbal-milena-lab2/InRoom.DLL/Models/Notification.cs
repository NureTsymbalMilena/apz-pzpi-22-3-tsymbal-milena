using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InRoom.DLL.Models;

public class Notification
{
    [Key]
    public Guid NotificationId { get; set; }
    [ForeignKey("User")]
    public Guid  UserId { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Message { get; set; }
}
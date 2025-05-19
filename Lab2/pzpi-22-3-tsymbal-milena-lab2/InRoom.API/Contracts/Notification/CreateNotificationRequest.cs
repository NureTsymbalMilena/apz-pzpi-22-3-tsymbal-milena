using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.Notification;

public class CreateNotificationRequest
{
    [Required]
    public Guid UserId { get; set; }
        
    [Required]
    public string Message { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.Notification;

public class UpdateNotificationRequest
{
    [Required]
    public string Message { get; set; }
}
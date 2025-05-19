using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts;

public class CreateMovementRequest
{
    [Required]
    public Guid DeviceId { get; set; }
        
    [Required]
    public Guid UserId { get; set; }
}
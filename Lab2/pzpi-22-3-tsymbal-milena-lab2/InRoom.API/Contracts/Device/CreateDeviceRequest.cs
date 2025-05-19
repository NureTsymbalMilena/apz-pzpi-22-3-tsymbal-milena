using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.Device;

public class CreateDeviceRequest
{
    [Required]
    public string Model { get; set; }
        
    [Required]
    public string RoomName { get; set; }
}
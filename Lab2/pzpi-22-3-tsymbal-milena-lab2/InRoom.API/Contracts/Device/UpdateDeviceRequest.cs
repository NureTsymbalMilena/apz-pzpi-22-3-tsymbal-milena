using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.Device;

public class UpdateDeviceRequest
{
    [Required]
    public string Model { get; set; }
        
    [Required]
    public string RoomName { get; set; }
}
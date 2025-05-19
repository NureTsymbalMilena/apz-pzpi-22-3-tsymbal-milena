using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.Room;

public class UpdateRoomDimensionsRequest
{
    [Required]
    public float X { get; set; }
    
    [Required]
    public float Y { get; set; }
    
    [Required]
    public float Z { get; set; }
}
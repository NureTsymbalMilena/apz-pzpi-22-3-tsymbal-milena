using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.User;

public class UpdateUserLocationRequest
{
    [Required]
    public float X { get; set; }
    
    [Required]
    public float Y { get; set; }
    
    [Required]
    public float Z { get; set; }
    
}
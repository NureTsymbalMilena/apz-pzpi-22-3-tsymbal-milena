using System.ComponentModel.DataAnnotations;
using InRoom.DLL.Enums;

namespace InRoom.API.Contracts.Admin;

public class ConnectUserToDeviceRequest
{
    [Required]
    [EmailAddress]
    public string UserEmail { get; set; }
    
    [Required]
    public string RoomName { get; set; }
}
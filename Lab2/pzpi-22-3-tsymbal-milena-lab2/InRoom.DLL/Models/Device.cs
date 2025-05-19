using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InRoom.DLL.Enums;

namespace InRoom.DLL.Models;

public class Device
{
    [Key]
    public Guid DeviceId { get; set; }
    [ForeignKey("Room")]
    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    public string Model { get; set; }
    public DeviceStatuses Status { get; set; } = DeviceStatuses.Inactive;
    public bool SignalReceivingEnabled { get; set; } = false;
}
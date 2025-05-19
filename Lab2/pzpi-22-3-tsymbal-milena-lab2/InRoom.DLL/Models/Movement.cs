using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InRoom.DLL.Models;

public class Movement
{
    [Key]
    public Guid MovementId { get; set; }
    [ForeignKey("Device")]
    public Guid DeviceId { get; set; }
    public Device Device { get; set; }
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime EnterTime { get; set; } = DateTime.Now;
    public DateTime? ExitTime { get; set; }
}
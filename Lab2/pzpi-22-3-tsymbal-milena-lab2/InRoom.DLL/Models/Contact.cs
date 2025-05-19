using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InRoom.DLL.Models;

public class Contact
{
    [Key]
    public Guid ContactId { get; set; }
    [ForeignKey("User")]
    public Guid ContactInitiatorId { get; set; }
    public User ContactInitiator { get; set; }
    [ForeignKey("User")]
    public Guid ContactReceiverId { get; set; }
    public User ContactReceiver { get; set; }
    [ForeignKey("Device")]
    public Guid DeviceId { get; set; }
    public Device Device{ get; set; }
    public DateTime ContactStartTime { get; set; } = DateTime.Now;
    public DateTime? ContactEndTime { get; set; }
    public double MinDistance { get; set; }  = default( double );
}
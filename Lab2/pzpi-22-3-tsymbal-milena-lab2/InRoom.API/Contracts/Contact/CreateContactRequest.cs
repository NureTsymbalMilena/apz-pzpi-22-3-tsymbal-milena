using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.Contact;

public class CreateContactRequest
{
    [Required]
    public Guid InitiatorId { get; set; }
        
    [Required]
    public Guid ReceiverId { get; set; }
    
    [Required]
    public Guid DeviceId { get; set; }
    
    [Required]
    public float InitiatorX { get; set; }
    
    [Required]
    public float InitiatorY { get; set; }
    
    [Required]
    public float InitiatorZ { get; set; }
    
    [Required]
    public float ReceiverX { get; set; }
    
    [Required]
    public float ReceiverY { get; set; }
    
    [Required]
    public float ReceiverZ { get; set; }
}
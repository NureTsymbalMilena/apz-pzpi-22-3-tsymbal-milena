using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.Contact;

public class UpdateContactRequest
{
    [Required]
    public float Distance { get; set; }
}
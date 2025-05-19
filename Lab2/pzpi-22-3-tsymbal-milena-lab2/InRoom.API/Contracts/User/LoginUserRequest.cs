using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.User;

public class LoginUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
        
    [Required]
    public string Password { get; set; }
}
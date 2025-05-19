using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.User;

public class RegisterUserRequest
{
    [Required]
    public string Name { get; set; }
        
    [Required]
    public string Surname { get; set; }
        
    [Required]
    [EmailAddress]
    public string Email { get; set; }
        
    [Required]
    public string Password { get; set; }
        
    [Required]
    public string HospitalName { get; set; }
}
using System.ComponentModel.DataAnnotations;
using InRoom.DLL.Enums;

namespace InRoom.API.Contracts.User;

public class UpdateUserRequest
{
    [Required]
    public string Name { get; set; }
        
    [Required]
    public string Surname { get; set; }
        
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string DiseaseName { get; set; }
        
}
using System.ComponentModel.DataAnnotations;
using InRoom.DLL.Enums;

namespace InRoom.API.Contracts.Admin;

public class SetRoleRequest
{
    [Required]
    [EmailAddress]
    public string UserEmail { get; set; }
    
    [Required]
    public Roles Role { get; set; }
}
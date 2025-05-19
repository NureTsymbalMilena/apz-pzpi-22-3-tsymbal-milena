using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InRoom.DLL.Enums;

namespace InRoom.DLL.Models;

public class User
{
    [Key]
    public Guid UserId { get; set; }
    [ForeignKey("Hospital")]
    public Guid HospitalId { get; set; }
    public Hospital? Hospital { get; set; }
    [ForeignKey("Device")]
    public Guid? DeviceId { get; set; }
    public Device? Device { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;   
    public Roles Role { get; set; } = Roles.User;
    public Guid? DiseaseId { get; set; }
    public Disease? Disease { get; set; } 
    public string Password { get; set; }
    public float X { get; set; } = 0;
    public float Y { get; set; } = 0;
    public float Z { get; set; } = 0;
}
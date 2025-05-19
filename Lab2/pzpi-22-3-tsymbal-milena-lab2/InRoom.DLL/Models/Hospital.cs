using System.ComponentModel.DataAnnotations;

namespace InRoom.DLL.Models;

public class Hospital
{
    [Key]
    public Guid HospitalId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public float X { get; set; } = 0;
    public float Y { get; set; } = 0;
    public float Z { get; set; } = 0;
}
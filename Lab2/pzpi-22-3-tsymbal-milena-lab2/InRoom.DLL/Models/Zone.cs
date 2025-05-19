using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InRoom.DLL.Enums;

namespace InRoom.DLL.Models;

public class Zone
{
    [Key]
    public Guid ZoneId { get; set; }
    [ForeignKey("Hospital")]
    public Guid HospitalId { get; set; }
    public Hospital Hospital { get; set; }
    public string Name { get; set; }
    public AccessLevels AccessLevel { get; set; } = AccessLevels.Open;
    public int FloorNumber { get; set; }
    public float Length { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float X { get; set; } = 0;
    public float Y { get; set; } = 0;
    public float Z { get; set; } = 0;
}
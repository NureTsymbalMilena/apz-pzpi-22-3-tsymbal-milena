using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InRoom.DLL.Models;

public class Room
{
    [Key]
    public Guid RoomId { get; set; }
    [ForeignKey("Zone")]
    public Guid ZoneId { get; set; }
    public Zone Zone { get; set; }
    public string Name { get; set; }
    public float Length { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float X { get; set; } = 0;
    public float Y { get; set; } = 0;
    public float Z { get; set; } = 0;
}
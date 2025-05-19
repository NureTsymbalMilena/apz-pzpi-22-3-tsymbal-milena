namespace InRoom.BLL.Contracts.User;

public class UserLocation
{
    public Guid UserId { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}
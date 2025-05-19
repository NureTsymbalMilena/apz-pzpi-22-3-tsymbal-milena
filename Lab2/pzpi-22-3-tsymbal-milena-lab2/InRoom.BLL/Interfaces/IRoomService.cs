using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface IRoomService: IGenericService<Room>
{
    Task<Room> Add(string roomName, string zoneName, float height, float width, float length);
    Task<Room> Update(Guid roomId, string roomName, string zoneName, float height, float width, float length);
    Task<Room> UpdateLocation(Guid roomId, float x, float y, float z);
}
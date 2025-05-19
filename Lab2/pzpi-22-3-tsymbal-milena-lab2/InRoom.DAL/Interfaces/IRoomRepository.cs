using InRoom.DLL.Models;

namespace InRoom.DAL.Interfaces;

public interface IRoomRepository: IRepository<Room>
{
    Task<Room?> GetByName(string roomName);
}
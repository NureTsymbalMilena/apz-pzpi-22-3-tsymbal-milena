using InRoom.DLL.Models;

namespace InRoom.DAL.Interfaces;

public interface IZoneRepository: IRepository<Zone>
{
    Task<Zone?> GetByName(string zoneName);
}
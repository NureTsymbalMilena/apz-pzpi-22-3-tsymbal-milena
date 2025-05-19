using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface IZoneService: IGenericService<Zone>
{
    Task<Zone> Add(string zoneName, int floorNumber, string hospitalName, float height, float width, float length);
    Task<Zone> Update(Guid zoneId, string zoneName, int floorNumber, string hospitalName, float height, float width, float length);
    Task<Zone> UpdateLocation(Guid zoneId, float x, float y, float z);
}
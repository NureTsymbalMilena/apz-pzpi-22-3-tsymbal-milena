using InRoom.BLL.Contracts.Room;
using InRoom.BLL.Contracts.User;
using InRoom.DLL.Enums;
using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface IDeviceService: IGenericService<Device>
{
    Task<Device> Add(string model, string roomName);
    Task<Device> Update(Guid deviceId, string model, string roomName);
    Task<RoomLocation> GetLocationById(Guid deviceId);
    Task<Device> UpdateStatus(Guid deviceId, DeviceStatuses deviceStatus);
    Task<UsersInZoneResponse> GetUsersInZoneById(Guid deviceId);
}
using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL.Repositories;

public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
{
    private readonly ApplicationDbContext _context;

    // Constructor that accepts the ApplicationDbContext and passes it to the base repository
    public DeviceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    // Method to retrieve a device by the name of the room it is associated with
    public async Task<Device?> GetByRoomName(string roomName)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(device => device.Room.Name == roomName);
        
        return device;
    }
    
    new public async Task<Device?> GetById(Guid deviceId)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(device => device.DeviceId == deviceId);
        
        device.Room = await _context.Rooms.FirstOrDefaultAsync(room => room.RoomId == device.RoomId);
        
        return device;
    }
}

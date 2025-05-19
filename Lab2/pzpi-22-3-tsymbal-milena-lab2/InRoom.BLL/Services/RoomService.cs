using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;

namespace InRoom.BLL.Services;

public class RoomService: GenericService<Room>, IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomRepository _roomRepository;
    private readonly IZoneRepository _zoneRepository;

    // Constructor to inject the required dependencies: UnitOfWork, RoomRepository, and ZoneRepository
    public RoomService(IUnitOfWork unitOfWork, IRoomRepository roomRepository, IZoneRepository zoneRepository) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _roomRepository = roomRepository;
        _zoneRepository = zoneRepository;
    }
    
    // Method to add a new room to a specific zone
    public async Task<Room> Add(string roomName, string zoneName, float height, float width, float length)
    {
        var room = await _roomRepository.GetByName(roomName);
        if (room != null)
        {
            throw new ApiException($"Room with name {roomName} is already in use", 400);
        }
        
        var zone = await _zoneRepository.GetByName(zoneName);
        if (zone == null)
        {
            throw new ApiException($"Zone with name {zoneName} not found", 404);
        }
        
        if (height <= 0 || width <= 0 || length <= 0)
        {
            throw new ApiException($"Room {roomName} has invalid dimensions: height, width, and length must be greater than zero.", 400);
        }

        var newRoom = new Room()
        {
            RoomId = Guid.NewGuid(),
            Name = roomName,
            ZoneId = zone.ZoneId,
            Zone = zone,
            Height = height,
            Width = width,
            Length = length
        };
      
        await Repository.Add(newRoom);
        await _unitOfWork.SaveChangesAsync();

        return newRoom;
    }

    // Method to update an existing room's details
    public async Task<Room> Update(Guid roomId, string roomName, string zoneName, float height, float width, float length)
    {
        var room = await Repository.GetById(roomId);
        if (room == null)
        {
            throw new ApiException($"Room with ID {roomId} not found.", 404);
        }
        
        var zone = await _zoneRepository.GetByName(zoneName);
        if (zone == null)
        {
            throw new ApiException($"Zone with name {zoneName} not found", 404);
        }
        
        if (height <= 0 || width <= 0 || length <= 0)
        {
            throw new ApiException($"Room {roomName} has invalid dimensions: height, width, and length must be greater than zero.", 400);
        }

        room.Name = roomName;
        room.ZoneId = zone.ZoneId;
        room.Zone = zone;
        room.Height = height;
        room.Width = width;
        room.Length = length;

        await Repository.Update(room);
        await _unitOfWork.SaveChangesAsync();

        return room;
    }
    
    // Method to update an existing room's dimensions
    public async Task<Room> UpdateLocation(Guid roomId, float x, float y, float z)
    {
        var room = await Repository.GetById(roomId);
        if (room == null)
        {
            throw new ApiException($"Room with ID {roomId} not found.", 404);
        }
        
        room.X = x;
        room.Y = y;
        room.Z = z;
        
        await Repository.Update(room);
        await _unitOfWork.SaveChangesAsync();

        return room;
    }
}

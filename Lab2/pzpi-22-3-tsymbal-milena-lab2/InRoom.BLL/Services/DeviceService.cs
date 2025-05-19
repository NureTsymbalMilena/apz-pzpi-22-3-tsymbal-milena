using InRoom.BLL.Contracts.Room;
using InRoom.BLL.Contracts.User;
using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Enums;
using InRoom.DLL.Models;

namespace InRoom.BLL.Services;

public class DeviceService: GenericService<Device>, IDeviceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IZoneRepository _zoneRepository;
    private readonly IUserRepository _userRepository;

    // Constructor to inject required dependencies for the DeviceService
    public DeviceService(
        IUnitOfWork unitOfWork, 
        IDeviceRepository deviceRepository, 
        IRoomRepository roomRepository, 
        IZoneRepository zoneRepository,
        IUserRepository userRepository) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _deviceRepository = deviceRepository;
        _roomRepository = roomRepository;
        _zoneRepository = zoneRepository;
        _userRepository = userRepository;
    }
    
    // Method to add a new device to a room, ensuring the room is available
    public async Task<Device> Add(string model, string roomName)
    {
        var device = await _deviceRepository.GetByRoomName(roomName);
        if (device != null)
        {
            throw new ApiException($"The device in room {roomName} is currently in use", 400);
        }

        var room = await _roomRepository.GetByName(roomName);
        if (room == null)
        {
            throw new ApiException($"The room {roomName} wasn't found", 404);
        }
        
        var newDevice = new Device
        {
            DeviceId = Guid.NewGuid(),
            Model = model,
            RoomId = room.RoomId,
            Room = room
        };
      
        await Repository.Add(newDevice);
        await _unitOfWork.SaveChangesAsync();

        return newDevice;
    }

    // Method to update an existing device with a new model and room
    public async Task<Device> Update(Guid deviceId, string model, string roomName)
    {
        var device = await Repository.GetById(deviceId);
        if (device == null)
        {
            throw new ApiException($"Device with ID {deviceId} not found.", 404);
        }
        
        var room = await _roomRepository.GetByName(roomName);
        if (room == null)
        {
            throw new ApiException($"The room {roomName} wasn't found", 404);
        }

        device.Model = model;
        device.RoomId = room.RoomId;
        device.Room = room;

        await Repository.Update(device);
        await _unitOfWork.SaveChangesAsync();

        return device;
    }
    
    // Method to get device location by id
    public async Task<RoomLocation> GetLocationById(Guid deviceId)
    {
        var device = await _deviceRepository.GetById(deviceId);
        if (device == null)
        {
            throw new ApiException($"Device with ID {deviceId} not found.", 404);
        }

        var roomLocation = new RoomLocation()
        {
            X = device.Room.X,
            Y = device.Room.Y,
            Z = device.Room.Z,
            Height = device.Room.Height,
            Width = device.Room.Width,
            Length = device.Room.Length,
        };
        
        return roomLocation;
    }
    
    // Method to update an existing device with a new model status
    public async Task<Device> UpdateStatus(Guid deviceId, DeviceStatuses deviceStatus)
    {
        var device = await Repository.GetById(deviceId);
        if (device == null)
        {
            throw new ApiException($"Device with ID {deviceId} not found.", 404);
        }
        
        device.Status = deviceStatus;

        await Repository.Update(device);
        await _unitOfWork.SaveChangesAsync();

        return device;
    }
    
    // Method to update an existing device with a new model status
    public async Task<UsersInZoneResponse> GetUsersInZoneById(Guid deviceId)
    {
        var device = await Repository.GetById(deviceId);
        if (device == null)
        {
            throw new ApiException($"Device with ID {deviceId} not found.", 404);
        }
        
        var room = await _roomRepository.GetById(device.RoomId);
        if (room == null)
        {
            throw new ApiException($"Room for device with ID {deviceId} not found.", 404);
        }
        
        var zone = await _zoneRepository.GetById(room.ZoneId);
        if (zone == null)
        {
            throw new ApiException($"Zone for room with ID {room.RoomId} not found.", 404);
        }
        
        var allUsers = await _userRepository.GetUsersByHospitalId(zone.HospitalId);
        
        var usersInZone = new List<UserLocation>();

        foreach (var user in allUsers)
        {
            if (IsUserInZone(user, zone))
            {
                usersInZone.Add(new UserLocation
                {
                    UserId = user.UserId,
                    X = user.X,
                    Y = user.Y,
                    Z = user.Z
                });
            }
        }

        var userInZoneResponse = new UsersInZoneResponse()
        {
            Users = usersInZone
        };
        
        return userInZoneResponse;
    }
    
    private bool IsUserInZone(User user, Zone zone)
    {
        float halfLength = zone.Length / 2;
        float halfWidth = zone.Width / 2;
        float halfHeight = zone.Height / 2;

        return user.X >= (zone.X - halfLength) && user.X <= (zone.X + halfLength) &&
               user.Y >= (zone.Y - halfWidth) && user.Y <= (zone.Y + halfWidth) &&
               user.Z >= (zone.Z - halfHeight) && user.Z <= (zone.Z + halfHeight);
    }
}

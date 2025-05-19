using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DAL.Repositories;
using InRoom.DLL.Enums;
using InRoom.DLL.Models;

namespace InRoom.BLL.Services;

public class NotificationService: GenericService<Notification>, INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IRoomRepository _roomRepository;

    // Constructor to inject the required dependencies: UnitOfWork and UserRepository
    public NotificationService(
        IUnitOfWork unitOfWork, 
        INotificationRepository notificationRepository, 
        IUserRepository userRepository, 
        IDeviceRepository deviceRepository, 
        IRoomRepository roomRepository) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _deviceRepository = deviceRepository;
        _roomRepository = roomRepository;
    }
    
    // Method to add a new notification for a user with a specific message
    public async Task<Notification> Add(Guid userId, string message)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            throw new ApiException($"User with ID {userId} not found", 404);
        }

        var newNotification = new Notification()
        {
            NotificationId = Guid.NewGuid(),
            UserId = userId,
            User = user,
            Message = message
        };
      
        await Repository.Add(newNotification);
        await _unitOfWork.SaveChangesAsync();

        return newNotification;
    }
    
    // Method to add the range of messages 
    public async Task AddRange(Device device, User user)
    {
        if (user.DeviceId.HasValue)
        {
            user.Device = await _deviceRepository.GetById(user.DeviceId.Value);
            user.Device.Room = await _roomRepository.GetById(user.Device.RoomId);
        }
            
        device.Room = await _roomRepository.GetById(device.RoomId);
            
        var users = await _userRepository.GetUsersByHospitalId(user.HospitalId);
        var notifications = new List<Notification>();

        foreach (var admin in users.Where(u => u.Role == Roles.HospitalAdmin).Where(u => u.HospitalId == user.HospitalId))
        {
            notifications.Add(new Notification()
            {
                NotificationId = Guid.NewGuid(),
                UserId = admin.UserId,
                User = admin,
                Message = $"User {user.Name} {user.Surname} ({user.Email}) is in room {device.Room.Name}, " +
                          "different from their registered room " + $"{user.Device.Room.Name}"
            });
        }
            
        notifications.Add(new Notification()
        {
            NotificationId = Guid.NewGuid(),
            UserId = user.UserId,
            User = user,
            Message = $"You are registered being in room " +
                      $"{device.Room.Name}, different from the registered room " +
                      $"{user.Device.Room.Name}. Please, get back to the room {user.Device.Room.Name}"
        });

        if (notifications.Any())
        {
            await _notificationRepository.AddRange(notifications);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    // Method to update the message of an existing notification
    public async Task<Notification> Update(Guid notificationId, string message)
    {
        var notification = await Repository.GetById(notificationId);
        if (notification == null)
        {
            throw new ApiException($"Notification with ID {notificationId} not found", 404);
        }

        notification.Message = message;

        await Repository.Update(notification);
        await _unitOfWork.SaveChangesAsync();

        return notification;
    }
}

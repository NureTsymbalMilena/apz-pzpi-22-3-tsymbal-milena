using InRoom.BLL.Contracts.Movement;
using InRoom.BLL.Contracts.User;
using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Enums;
using InRoom.DLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace InRoom.BLL.Services;

public class MovementService: GenericService<Movement>, IMovementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMovementRepository _movementRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly INotificationService _notificationService;

    // Constructor to inject the required dependencies: UnitOfWork, MovementRepository, UserRepository, and DeviceRepository
    public MovementService(
        IUnitOfWork unitOfWork, 
        IMovementRepository movementRepository, 
        IUserRepository userRepository, 
        IDeviceRepository deviceRepository,
        INotificationService notificationService) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _movementRepository = movementRepository;
        _userRepository = userRepository;
        _deviceRepository = deviceRepository;
        _notificationService = notificationService;
    }
    
    // Method to add a new movement record for a user and a device
    public async Task<Movement> Add(Guid deviceId, Guid userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            throw new ApiException($"User with ID {userId} not found.", 404);
        }

        var device = await _deviceRepository.GetById(deviceId);
        if (device == null)
        {
            throw new ApiException($"Device with ID {deviceId} not found.", 404);
        }
        
        var newMovement = new Movement()
        {
            MovementId = Guid.NewGuid(),
            UserId = userId,
            User = user,
            DeviceId = deviceId,
            Device = device
        };
        
        await Repository.Add(newMovement);
        await _unitOfWork.SaveChangesAsync();

        if (!IsUserInRoom(user, device.DeviceId))
        {
            await _notificationService.AddRange(device, user);
        }
        
        return newMovement;
    }

    private bool IsUserInRoom(User user, Guid deviceId)
    {
        return user.Role == Roles.User && user.DeviceId != null && user.DeviceId == deviceId;
    }

    // Method to update the exit time of an existing movement
    public async Task<Movement> Update(Guid movementId)
    {
        var movement = await Repository.GetById(movementId);
        if (movement == null)
        {
            throw new ApiException($"Movement with ID {movementId} not found.", 404);
        }

        movement.ExitTime = DateTime.Now;

        await Repository.Update(movement);
        await _unitOfWork.SaveChangesAsync();

        return movement;
    }
    
    // Method to get movements report
    public async Task<MovementsReportResponse> GetMovementsReport(Guid userId, DateTime startDate, DateTime endDate)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            throw new ApiException($"User with ID {userId} not found.", 404);
        }

        var movements = await _movementRepository.GetMovementsByDateRangeAndUserId(userId, startDate, endDate);
      
        var movementsReportResponse = new MovementsReportResponse()
        {
            User = user,
            StartDate = startDate,
            EndDate = endDate,
            Movements = movements
        };
        
        return movementsReportResponse;
    }

    public async Task<List<Movement>> GetDeviceMovements(Guid deviceId)
    {
        var device = await _deviceRepository.GetById(deviceId);
        if (device == null)
        {
            throw new ApiException($"Device with ID {deviceId} not found.", 404);
        }
        
        var movements = await _movementRepository.GetMovementsByDeviceId(deviceId);
        
        return movements;
    }
}

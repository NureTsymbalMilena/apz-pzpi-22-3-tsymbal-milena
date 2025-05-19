using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL.Repositories;

public class MovementRepository: GenericRepository<Movement>, IMovementRepository
{
    private readonly ApplicationDbContext _context;
    
    // Constructor that initializes the repository with the ApplicationDbContext
    public MovementRepository(ApplicationDbContext context): base(context)
    {
        _context = context;
    }

    // Method to get movements from specific period
    public async Task<List<Movement>> GetMovementsByDateRangeAndUserId(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await _context.Movements
            .Where(c => c.UserId == userId && c.EnterTime >= startDate && c.EnterTime <= endDate)
            .ToListAsync();
    }
    
    public async Task<List<Movement>> GetMovementsByDeviceId(Guid deviceId)
    {
        return await _context.Movements
            .Where(c => c.DeviceId == deviceId)
            .ToListAsync();
    }
}
using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL.Repositories;

public class RequestLogRepository: GenericRepository<RequestLog>, IRequestLogRepository
{
    private readonly ApplicationDbContext _context;
    
    // Constructor that initializes the repository with the ApplicationDbContext
    public RequestLogRepository(ApplicationDbContext context):base(context)
    {
        _context = context;
    }
    
    public async Task AddRange(List<RequestLog> requestLogs)
    {
        if (requestLogs == null || !requestLogs.Any())
        {
            throw new ArgumentException("Request logs collection is null or empty.", nameof(requestLogs));
        }

        await _context.RequestLogs.AddRangeAsync(requestLogs);
        await _context.SaveChangesAsync();
    }
    
    // Method to get request logs from specific period
    public async Task<List<RequestLog>> GetRequestLogsByDateRange(DateTime startDate, DateTime endDate)
    {
        return await _context.RequestLogs
            .Where(r => r.Timestamp >= startDate && r.Timestamp <= endDate)
            .ToListAsync();
    }
}
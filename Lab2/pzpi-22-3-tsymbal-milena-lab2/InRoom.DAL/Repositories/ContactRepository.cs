using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL.Repositories;

public class ContactRepository : GenericRepository<Contact>, IContactRepository
{
    private readonly ApplicationDbContext _context;
    
    // Constructor that accepts the ApplicationDbContext and passes it to the base repository
    public ContactRepository(ApplicationDbContext context): base(context)
    {
        _context = context;
    }
    
    // Method to get contacts from specific period
    public async Task<List<Contact>> GetContactsByDateRangeAndUserId(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await _context.Contacts
            .Where(c => c.ContactInitiatorId == userId || c.ContactStartTime >= startDate && c.ContactStartTime <= endDate &&
                        c.ContactReceiverId == userId)
            .ToListAsync();
    }
    
    // Method to get contacts of specific user
    public async Task<List<Contact>> GetByUserIdAndDaysNumber(Guid userId, int daysAgo = 14)
    {
        var startDate = DateTime.UtcNow.AddDays(-daysAgo); 
    
        return await _context.Contacts
            .Where(c => (c.ContactInitiatorId == userId || c.ContactReceiverId == userId) 
                        && c.ContactStartTime >= startDate)
            .ToListAsync();
    }
    
}
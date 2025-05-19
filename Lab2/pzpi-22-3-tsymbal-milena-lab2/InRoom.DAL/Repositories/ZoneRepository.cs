using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL.Repositories;

public class ZoneRepository : GenericRepository<Zone>, IZoneRepository
{
    private readonly ApplicationDbContext _context;
    
    // Constructor to initialize the ZoneRepository with the provided ApplicationDbContext
    public ZoneRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    // Method to retrieve a zone by its name
    public async Task<Zone?> GetByName(string zoneName)
    {
        var zone = await _context.Zones.FirstOrDefaultAsync(zone => zone.Name == zoneName);
        return zone; 
    }
}

using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context; 

    // Constructor to initialize the UserRepository with the provided ApplicationDbContext
    public UserRepository(ApplicationDbContext context) : base(context) 
    {
        _context = context;
    }

    // Method to retrieve a user by their email address
    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(c => c.Email == email); 
    }

    public async Task<List<User>> GetUsersByDiseaseId(Guid diseaseId)
    {
        return await _context.Users
            .Where(u => u.DiseaseId == diseaseId)
            .ToListAsync();
    }
    
    public async Task<List<User>> GetUsersByHospitalId(Guid hospitalId)
    {
        return await _context.Users
            .Where(u => u.HospitalId == hospitalId)
            .ToListAsync();
    }
    
}

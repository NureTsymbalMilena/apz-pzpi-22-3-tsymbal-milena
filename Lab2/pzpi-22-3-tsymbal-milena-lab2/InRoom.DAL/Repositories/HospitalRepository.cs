using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InRoom.DAL.Repositories;

public class HospitalRepository : GenericRepository<Hospital>, IHospitalRepository
{
    private readonly ApplicationDbContext _context;

    // Constructor that initializes the repository with the ApplicationDbContext
    public HospitalRepository(ApplicationDbContext context) : base(context)
    {
        _context = context; 
    }

    // Method to retrieve a hospital by its name
    public async Task<Hospital?> GetByName(string hospitalName)
    {
        var hospital = await _context.Hospitals.FirstOrDefaultAsync(hospital => hospital.Name == hospitalName);
        return hospital;
    }
}

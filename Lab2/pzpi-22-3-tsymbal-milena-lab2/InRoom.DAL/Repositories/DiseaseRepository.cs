using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL.Repositories;

public class DiseaseRepository: GenericRepository<Disease>, IDiseaseRepository
{
    private readonly ApplicationDbContext _context;
    
    // Constructor that initializes the repository with the ApplicationDbContext
    public DiseaseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context; 
    }
    
    // Method to retrieve a disease by its name
    public async Task<Disease?> GetByName(string diseaseName)
    {
        var disease = await _context.Diseases.FirstOrDefaultAsync(disease => disease.Name == diseaseName);
        return disease;
    }
}
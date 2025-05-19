using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Enums;
using InRoom.DLL.Models;

namespace InRoom.BLL.Services;

public class UserService : GenericService<User>, IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDiseaseRepository _diseaseRepository;
    public UserService(IUnitOfWork unitOfWork, IDiseaseRepository diseaseRepository) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _diseaseRepository = diseaseRepository;
    }
    
    // Method to update the user details (name, surname, email, and disease)
    public async Task<User> Update(Guid userId, string name, string surname, string email, string diseaseName)
    {
        var user = await Repository.GetById(userId);
        if (user == null)
        {
            throw new ApiException($"User with ID {userId} not found.", 404);
        }
        
        var disease = await _diseaseRepository.GetByName(diseaseName);
        if (disease == null)
        {
            throw new ApiException($"Disease with name {diseaseName} not found.", 404);
        }
        
        user.Name = name;
        user.Surname = surname;
        user.Email = email;
        user.DiseaseId = disease.DiseaseId;
        user.Disease = disease;
      
        await Repository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return user;
    }

    // Method to update the user location
    public async Task<User> UpdateLocation(Guid userId, float x, float y, float z)
    {
        var user = await Repository.GetById(userId);
        if (user == null)
        {
            throw new ApiException($"User with ID {userId} not found.", 404);
        }

        user.X = x;
        user.Y = y;
        user.Z = z;
        
        await Repository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return user;
    }
}

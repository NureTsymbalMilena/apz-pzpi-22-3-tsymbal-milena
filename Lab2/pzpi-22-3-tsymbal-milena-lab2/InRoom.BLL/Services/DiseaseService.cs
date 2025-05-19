using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Enums;
using InRoom.DLL.Models;

namespace InRoom.BLL.Services;

public class DiseaseService : GenericService<Disease>, IDiseaseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDiseaseRepository _diseaseRepository;
    private readonly IUserRepository _userRepository;
    
    // Constructor to inject the required UnitOfWork and DiseaseRepository dependencies
    public DiseaseService(IUnitOfWork unitOfWork, IDiseaseRepository diseaseRepository, IUserRepository userRepository) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _diseaseRepository = diseaseRepository;
        _userRepository = userRepository;
    }
    
    // Method to add a new disease
    public async Task<Disease> Add(
        string diseaseName, 
        SeverityLevel severityLevel, 
        bool contagious, 
        double contagionRate, 
        int incubationPeriod, 
        double mortalityRate, 
        TransmissionMode transmissionMode)
    {
        var disease = await _diseaseRepository.GetByName(diseaseName);
        if (disease != null)
        {
            throw new ApiException($"Disease with name {diseaseName} already exist", 400);
        }
        
        if (!Enum.IsDefined(typeof(SeverityLevel), severityLevel))
        {
            throw new ApiException("Invalid severity level", 400);
        }
        
        if (!Enum.IsDefined(typeof(TransmissionMode), transmissionMode))
        {
            throw new ApiException("Invalid transmission mode", 400);
        }

        var newDisease = new Disease()
        {
            DiseaseId = Guid.NewGuid(),
            Name = diseaseName,
            SeverityLevel = severityLevel,
            Contagious = contagious,
            ContagionRate = contagionRate,
            IncubationPeriod = incubationPeriod,
            MortalityRate = mortalityRate,
            TransmissionMode = transmissionMode,
        };
      
        await Repository.Add(newDisease);
        await _unitOfWork.SaveChangesAsync();

        return newDisease;
    }

    // Method to update an existing disease by its ID
    public async Task<Disease> Update(
        Guid diseaseId, 
        string diseaseName, 
        SeverityLevel severityLevel, 
        bool contagious, 
        double contagionRate, 
        int incubationPeriod, 
        double mortalityRate, 
        TransmissionMode transmissionMode)
    {
        var disease = await Repository.GetById(diseaseId);
        if (disease == null)
        {
            throw new ApiException($"Disease with ID {diseaseId} not found", 404);
        }
        
        if (!Enum.IsDefined(typeof(SeverityLevel), severityLevel))
        {
            throw new ApiException("Invalid severity level", 400);
        }
        
        if (!Enum.IsDefined(typeof(TransmissionMode), transmissionMode))
        {
            throw new ApiException("Invalid transmission mode", 400);
        }

        disease.Name = diseaseName;
        disease.SeverityLevel = severityLevel;
        disease.Contagious = contagious;
        disease.ContagionRate = contagionRate;
        disease.IncubationPeriod = incubationPeriod;
        disease.MortalityRate = mortalityRate;
        disease.TransmissionMode = transmissionMode;
        
        await Repository.Update(disease);
        await _unitOfWork.SaveChangesAsync();

        return disease;
    }
    
    public new async Task<Guid> Delete(Guid diseaseId)
    {
        var disease = await Repository.GetById(diseaseId);

        if (disease == null)
        {
            throw new ApiException("Desease wasn't found", 404);
        }

        var users = await _userRepository.GetUsersByDiseaseId(diseaseId);
        foreach (var user in users)
        {
            user.Disease = null;
            user.DiseaseId = null;
        }
        
        await Repository.Delete(diseaseId);

        await _unitOfWork.SaveChangesAsync();

        return diseaseId;
    }
}
using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Enums;
using InRoom.DLL.Models;

namespace InRoom.BLL.Services;

public class HospitalService: GenericService<Hospital>, IHospitalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHospitalRepository _hospitalRepository;
    
    // Constructor to inject the required UnitOfWork and HospitalRepository dependencies
    public HospitalService(IUnitOfWork unitOfWork, IHospitalRepository hospitalRepository) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _hospitalRepository = hospitalRepository;
    }
    
    // Method to add a new hospital
    public async Task<Hospital> Add(string hospitalName, string address)
    {
        var hospital = await _hospitalRepository.GetByName(hospitalName);
        if (hospital != null)
        {
            throw new ApiException($"Hospital with name {hospitalName} is in use", 400);
        }

        var newHospital = new Hospital()
        {
            HospitalId = Guid.NewGuid(),
            Name = hospitalName,
            Address = address
        };
      
        await Repository.Add(newHospital);
        await _unitOfWork.SaveChangesAsync();

        return newHospital;
    }

    // Method to update an existing hospital by its ID
    public async Task<Hospital> Update(Guid hospitalId, string hospitalName, string address)
    {
        var hospital = await Repository.GetById(hospitalId);
        if (hospital == null)
        {
            throw new ApiException($"Hospital with ID {hospitalId} not found.", 404);
        }

        hospital.Name = hospitalName;
        hospital.Address = address;

        await Repository.Update(hospital);
        await _unitOfWork.SaveChangesAsync();

        return hospital;
    }
}

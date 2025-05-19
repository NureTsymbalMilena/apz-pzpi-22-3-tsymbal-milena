using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;

namespace InRoom.BLL.Services;

public class ZoneService : GenericService<Zone>, IZoneService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IZoneRepository _zoneRepository;
    private readonly IHospitalRepository _hospitalRepository;

    public ZoneService(IUnitOfWork unitOfWork, IZoneRepository zoneRepository, IHospitalRepository hospitalRepository) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _zoneRepository = zoneRepository;
        _hospitalRepository = hospitalRepository;
    }

    // Method to add a new zone to a hospital
    public async Task<Zone> Add(string zoneName, int floorNumber, string hospitalName, float height, float width, float length)
    {
        var zone = await _zoneRepository.GetByName(zoneName);
        if (zone != null)
        {
            throw new ApiException($"Zone with name {zoneName} is in use", 400);
        }

        var hospital = await _hospitalRepository.GetByName(hospitalName);

        if (hospital == null)
        {
            throw new ApiException($"Hospital with name {hospitalName} is not found", 404);
        }

        if (height <= 0 || width <= 0 || length <= 0)
        {
            throw new ApiException($"Zone {zoneName} has invalid dimensions: height, width, and length must be greater than zero.", 400);
        }
        
        var newZone = new Zone()
        {
            ZoneId = Guid.NewGuid(),
            Name = zoneName,
            FloorNumber = floorNumber,
            HospitalId = hospital.HospitalId,
            Hospital = hospital,
            Height = height,
            Width = width,
            Length = length
        };

        await Repository.Add(newZone);
        await _unitOfWork.SaveChangesAsync();

        return newZone;
    }

    // Method to update an existing zone
    public async Task<Zone> Update(Guid zoneId, string zoneName, int floorNumber, string hospitalName, float height, float width, float length)
    {
        var zone = await Repository.GetById(zoneId);
        if (zone == null)
        {
            throw new ApiException($"Zone with ID {zoneId} not found.", 404);
        }

        var hospital = await _hospitalRepository.GetByName(hospitalName);
        if (hospital == null)
        {
            throw new ApiException($"Hospital with name {hospitalName} is not found", 404);
        }

        if (height <= 0 || width <= 0 || length <= 0)
        {
            throw new ApiException($"Zone {zoneName} has invalid dimensions: height, width, and length must be greater than zero.", 400);
        }
        
        zone.Name = zoneName;
        zone.FloorNumber = floorNumber;
        zone.HospitalId = hospital.HospitalId;
        zone.Hospital = hospital;
        zone.Height = height;
        zone.Width = width;
        zone.Length = length;

        await Repository.Update(zone);
        await _unitOfWork.SaveChangesAsync();

        return zone;
    }
    
    // Method to update an existing zone's location
    public async Task<Zone> UpdateLocation(Guid zoneId, float x, float y, float z)
    {
        var zone = await Repository.GetById(zoneId);
        if (zone == null)
        {
            throw new ApiException($"Zone with ID {zoneId} not found.", 404);
        }
        
        zone.X = x;
        zone.Y = y;
        zone.Z = z;

        await Repository.Update(zone);
        await _unitOfWork.SaveChangesAsync();

        return zone;
    }
}

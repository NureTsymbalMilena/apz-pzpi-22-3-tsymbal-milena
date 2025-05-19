using InRoom.DLL.Models;

namespace InRoom.DAL.Interfaces;

public interface IHospitalRepository : IRepository<Hospital>
{
    Task<Hospital?> GetByName(string hospitalName);
}
using InRoom.DLL.Models;

namespace InRoom.DAL.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmail(string email);
    Task<List<User>> GetUsersByDiseaseId(Guid diseaseId);
    Task<List<User>> GetUsersByHospitalId(Guid hospitalId);
    new Task<User?> GetById(Guid id);
}
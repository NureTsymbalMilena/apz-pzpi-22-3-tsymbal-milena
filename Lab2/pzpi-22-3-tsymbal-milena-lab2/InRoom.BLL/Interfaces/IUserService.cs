using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface IUserService : IGenericService<User>
{
    Task<User> Update(Guid userId, string name, string surname, string email, string diseaseName);
    Task<User> UpdateLocation(Guid userId, float X, float Y, float Z);
}
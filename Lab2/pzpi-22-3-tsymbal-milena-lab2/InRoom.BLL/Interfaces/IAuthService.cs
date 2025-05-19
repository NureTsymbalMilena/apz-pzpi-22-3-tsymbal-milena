using InRoom.BLL.Contracts.User;

namespace InRoom.BLL.Interfaces;

public interface IAuthService
{
    Task Register(string name, string surname, string email, string password, string hospitalName);
    Task<LoginUserResponse> Login(string email, string password);
}
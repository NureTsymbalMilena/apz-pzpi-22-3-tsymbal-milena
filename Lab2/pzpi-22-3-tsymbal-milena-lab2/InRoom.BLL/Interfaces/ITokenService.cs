using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface ITokenService
{
    Task<Tuple<string, string>> GenerateTokens(User user);
}
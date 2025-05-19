using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface IJwtProvider
{
    public string GenerateAccessToken(User user);
    public string GenerateRefreshToken(User user);
}
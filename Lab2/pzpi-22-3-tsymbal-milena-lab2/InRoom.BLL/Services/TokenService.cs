using InRoom.BLL.Interfaces;
using InRoom.DLL.Models;

namespace InRoom.BLL.Services;

public class TokenService(IJwtProvider jwtProvider) : ITokenService
{
    // Method to generate access and refresh tokens for a given user
    public Task<Tuple<string,string>> GenerateTokens(User user)
    {
        var accessToken = jwtProvider.GenerateAccessToken(user);

        var refreshToken = jwtProvider.GenerateRefreshToken(user);

        return Task.FromResult(Tuple.Create(accessToken, refreshToken));
    }
}

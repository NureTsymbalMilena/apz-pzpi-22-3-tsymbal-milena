using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InRoom.BLL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InRoom.BLL.Jwt;

public class JwtProvider(IOptions<JwtOptions> options): IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    
    public string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim("UserId", user.UserId.ToString()),
            new Claim("type", "access"),
            new Claim("Email", user.Email),
            new Claim("Role", user.Role.ToString()),
        }; 
        
        var signingCredentials = new SigningCredentials(  
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),  
            SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddHours(_options.AccessExpireMinutes));
  
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(accessToken);  
  
        return tokenValue;  
    }

    public string GenerateRefreshToken(User user)
    {
        var claims = new[]
        {
            new Claim("UserId", user.UserId.ToString()),
            new Claim("type", "refresh"),
            new Claim("Email", user.Email),
            new Claim("Role", user.Role.ToString()),
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var refreshToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddDays(_options.RefreshExpireHours));

        return new JwtSecurityTokenHandler().WriteToken(refreshToken);
    }
}
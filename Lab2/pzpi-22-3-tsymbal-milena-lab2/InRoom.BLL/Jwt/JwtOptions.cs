namespace InRoom.BLL.Jwt;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int RefreshExpireHours { get; set; }
    public int AccessExpireMinutes { get; set; }
}
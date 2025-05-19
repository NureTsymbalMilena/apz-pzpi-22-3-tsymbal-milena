namespace InRoom.BLL.Contracts.User;

public class LoginUserResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
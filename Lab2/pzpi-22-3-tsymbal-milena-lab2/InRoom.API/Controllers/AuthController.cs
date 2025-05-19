using InRoom.API.Contracts.User;
using InRoom.BLL.Contracts.User;
using InRoom.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    // Endpoint for registering a new user in the system
    [HttpPost("register")]
    [SwaggerOperation("User registration into the system")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        await _authService.Register(request.Name, request.Surname, request.Email, request.Password, request.HospitalName);
        return Ok(new { Message = "User successfully registered" });
    }

    // Endpoint for logging a user into the system
    [HttpPost("login")]
    [SwaggerOperation("User login into the system")]
    [ProducesResponseType(200, Type = typeof(LoginUserResponse))]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var loginResult = await _authService.Login(request.Email, request.Password);
        
        HttpContext.Response.Cookies.Append("tasty-cookies", loginResult.RefreshToken, new CookieOptions()
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(30),
            SameSite = SameSiteMode.Strict
        });

        return Ok(loginResult);
    }

    // Endpoint for logging out a user from the system
    [HttpPost("logout")]
    [SwaggerOperation("User logout from the system")]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Response.Cookies.Delete("tasty-cookies");
        return Ok(new { Message = "Successfully logged out" });
    }
}

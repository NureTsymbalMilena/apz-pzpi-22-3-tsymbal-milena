using InRoom.API.Contracts.User;
using InRoom.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    // Endpoint for retrieving a user by their ID
    [HttpGet("{userId:Guid}")]
    [SwaggerOperation("Get user by id")]
    public async Task<IActionResult> GetUser([FromRoute] Guid userId)
    {
        var user = await _userService.GetById(userId);
        return Ok(user);
    }
    
    // Endpoint for retrieving all users in the system
    [HttpGet]
    [SwaggerOperation("Get all users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }
    
    // Endpoint for updating a user's information by their ID
    [HttpPatch("{userId:Guid}")]
    [SwaggerOperation("Edit user by id")]
    public async Task<IActionResult> EditUser([FromRoute] Guid userId, UpdateUserRequest request)
    {
        var user = await _userService.Update(userId, request.Name, request.Email, request.Email, request.DiseaseName);
        return Ok(user);
    }
    
    // Endpoint for updating a user's location by their ID
    [HttpPatch("{userId:Guid}/location")]
    [SwaggerOperation("Edit user by id")]
    public async Task<IActionResult> EditUserLocation([FromRoute] Guid userId, UpdateUserLocationRequest request)
    {
        var user = await _userService.UpdateLocation(userId, request.X, request.Y, request.Z);
        return Ok(user);
    }
    
    // Endpoint for deleting a user by their ID
    [HttpDelete("{userId:Guid}")]
    [SwaggerOperation("Delete user by id")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        var user = await _userService.Delete(userId);
        return Ok(user);
    }
    
    // Endpoint for retrieving a user's location in the room
    [HttpGet("location/{userId:Guid}")]
    [SwaggerOperation("Get user location in the room")]
    public async Task<IActionResult> GetUserLocation([FromRoute] Guid userId)
    {
        return Ok(new { Message = $"User with ID {userId} is located" });
    }
}

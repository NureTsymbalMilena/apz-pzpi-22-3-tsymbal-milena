using InRoom.API.Contracts.Hospital;
using InRoom.API.Contracts.Room;
using InRoom.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }
    
    // Endpoint for creating a new room in the system
    [HttpPost]
    [SwaggerOperation("Create room")]
    public async Task<IActionResult> AddRoom(CreateRoomRequest request)
    {
        var room = await _roomService.Add(request.Name, request.ZoneName, request.Height, request.Width, request.Length);
        return Ok(room);
    }
    
    // Endpoint for retrieving a room by its ID
    [HttpGet("{roomId:Guid}")]
    [SwaggerOperation("Get room by id")]
    public async Task<IActionResult> GetRoom([FromRoute] Guid roomId)
    {
        var room = await _roomService.GetById(roomId);
        return Ok(room);
    }
    
    // Endpoint for retrieving all rooms in the system
    [HttpGet]
    [SwaggerOperation("Get all rooms")]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _roomService.GetAll();
        return Ok(rooms);
    }
    
    // Endpoint for updating room information by its ID
    [HttpPatch("{roomId:Guid}/details")]
    [SwaggerOperation("Edit room details by id")]
    public async Task<IActionResult> EditRoom([FromRoute] Guid roomId, UpdateRoomRequest request)
    {
        var room = await _roomService.Update(roomId, request.Name, request.ZoneName, request.Height, request.Width, request.Length);
        return Ok(room);
    }
    
    // Endpoint for updating room coordinates by its ID
    [HttpPatch("{roomId:Guid}/coordinates")]
    [SwaggerOperation("Edit room coordinates by id")]
    public async Task<IActionResult> EditRoomDimensions([FromRoute] Guid roomId, UpdateRoomDimensionsRequest request)
    {
        var room = await _roomService.UpdateLocation(roomId, request.X, request.Y, request.Z);
        return Ok(room);
    }
    
    // Endpoint for deleting a room by its ID
    [HttpDelete("{roomId:Guid}")]
    [SwaggerOperation("Delete room by id")]
    public async Task<IActionResult> DeleteRoom([FromRoute] Guid roomId)
    {
        var room = await _roomService.Delete(roomId);
        return Ok(room);
    }
}

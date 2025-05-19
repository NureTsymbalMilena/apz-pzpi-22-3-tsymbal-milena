using InRoom.API.Contracts;
using InRoom.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovementController : ControllerBase
{
    private readonly IMovementService _movementService;

    public MovementController(IMovementService movementService)
    {
        _movementService = movementService;
    }
    
    // Endpoint for creating a new movement by associating a device with a user
    [HttpPost]
    [SwaggerOperation("Create movement")]
    public async Task<IActionResult> AddMovement(CreateMovementRequest request)
    {
        var movement = await _movementService.Add(request.DeviceId, request.UserId);
        return Ok(movement);
    }
    
    // Endpoint for retrieving a movement by its ID
    [HttpGet("{movementId:Guid}")]
    [SwaggerOperation("Get movement by id")]
    public async Task<IActionResult> GetMovement([FromRoute] Guid movementId)
    {
        var movement = await _movementService.GetById(movementId);
        return Ok(movement);
    }
    
    // Endpoint for retrieving all movements
    [HttpGet]
    [SwaggerOperation("Get all movements")]
    public async Task<IActionResult> GetAllMovements()
    {
        var movements = await _movementService.GetAll();
        return Ok(movements);
    }
    
    // Endpoint for retrieving all movements of specific device
    [HttpGet("{deviceId:Guid}/device")]
    [SwaggerOperation("Get all movements of specific device")]
    public async Task<IActionResult> GetAllDeviceMovements([FromRoute] Guid deviceId)
    {
        var movements = await _movementService.GetDeviceMovements(deviceId);
        return Ok(movements);
    }
    
    // Endpoint for updating a movement by its ID
    [HttpPatch("{movementId:Guid}")]
    [SwaggerOperation("Edit movement by id")]
    public async Task<IActionResult> EditMovement([FromRoute] Guid movementId)
    {
        var movement = await _movementService.Update(movementId);
        return Ok(movement);
    }
    
    // Endpoint for deleting a movement by its ID
    [HttpDelete("{movementId:Guid}")]
    [SwaggerOperation("Delete movement by id")]
    public async Task<IActionResult> DeleteMovement([FromRoute] Guid movementId)
    {
        var movement = await _movementService.Delete(movementId);
        return Ok(movement);
    }
    
    // Endpoint for generating a movement report for a specific user and time period
    [HttpGet("{userId:Guid}/report")]
    [SwaggerOperation("Get movement report for a specific period")]
    public async Task<IActionResult> GenerateReport([FromRoute] Guid userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var movementsReport = await _movementService.GetMovementsReport(userId, startDate, endDate);
        return Ok(movementsReport);
    }
}
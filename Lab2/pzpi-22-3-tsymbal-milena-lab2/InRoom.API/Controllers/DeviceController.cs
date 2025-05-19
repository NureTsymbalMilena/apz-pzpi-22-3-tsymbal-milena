using InRoom.API.Contracts.Device;
using InRoom.API.Contracts.Hospital;
using InRoom.BLL.Interfaces;
using InRoom.DLL.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }
    
    // Endpoint for creating a new device with provided model and room name
    [HttpPost]
    [SwaggerOperation("Create device")]
    public async Task<IActionResult> AddDevice(CreateDeviceRequest request)
    {
        var device = await _deviceService.Add(request.Model, request.RoomName);
        return Ok(device);
    }
    
    // Endpoint for retrieving a device by its ID
    [HttpGet("{deviceId:Guid}")]
    [SwaggerOperation("Get device by id")]
    public async Task<IActionResult> GetDevice([FromRoute] Guid deviceId)
    {
        var device = await _deviceService.GetById(deviceId);
        return Ok(device);
    }
    
    // Endpoint for retrieving all devices
    [HttpGet]
    [SwaggerOperation("Get all devices")]
    public async Task<IActionResult> GetAllDevices()
    {
        var devices = await _deviceService.GetAll();
        return Ok(devices);
    }
    
    // Endpoint for retrieving a device location by its ID
    [HttpGet("{deviceId:Guid}/location")]
    [SwaggerOperation("Get device location by id")]
    public async Task<IActionResult> GetDeviceLocation([FromRoute] Guid deviceId)
    {
        var deviceLocation = await _deviceService.GetLocationById(deviceId);
        return Ok(deviceLocation);
    }
    
    // Endpoint for updating device information (model and room name) by its ID
    [HttpPatch("{deviceId:Guid}")]
    [SwaggerOperation("Edit device by id")]
    public async Task<IActionResult> EditDevice([FromRoute] Guid deviceId, UpdateDeviceRequest request)
    {
        var device = await _deviceService.Update(deviceId, request.Model, request.RoomName);
        return Ok(device);
    }
    
    // Endpoint for updating device status by its ID
    [HttpPatch("{deviceId:Guid}/status")]
    [SwaggerOperation("Edit device status by id")]
    public async Task<IActionResult> EditDeviceStatus([FromRoute] Guid deviceId, [FromBody] DeviceStatuses status)
    {
        var device = await _deviceService.UpdateStatus(deviceId, status);
        return Ok(device);
    }
    
    // Endpoint for deleting a device by its ID
    [HttpDelete("{deviceId:Guid}")]
    [SwaggerOperation("Delete device by id")]
    public async Task<IActionResult> DeleteDevice([FromRoute] Guid deviceId)
    {
        var device = await _deviceService.Delete(deviceId);
        return Ok(device);
    }
    
    // Endpoint for getting users in zone a device by its ID
    [HttpGet("{deviceId:Guid}/usersInZone")]
    [SwaggerOperation("Get users in zone by device id")]
    public async Task<IActionResult> GetUsersInZone([FromRoute] Guid deviceId)
    {
        var usersInZoneResponse = await _deviceService.GetUsersInZoneById(deviceId);
        return Ok(usersInZoneResponse);
    }
}

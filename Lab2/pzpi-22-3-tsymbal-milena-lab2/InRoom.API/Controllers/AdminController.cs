using InRoom.API.Contracts.Admin;
using InRoom.API.Contracts.User;
using InRoom.API.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DLL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IRequestLogService _requestLogService;

    public AdminController(IAdminService adminService, IRequestLogService requestLogService)
    {
        _adminService = adminService;
        _requestLogService = requestLogService;
    }
    
    // Endpoint for database backup
    [HttpPost("backup")]
    [SwaggerOperation("Backup database")]
    [RoleVerification(Roles.DatabaseAdmin)]
    public async Task<IActionResult> Backup([FromQuery] string outputDirectory = null)
    {
        string backupFilePath = await _adminService.BackupData(outputDirectory);
        return Ok(new { message = $"Backup successfully done at {backupFilePath}" });
    }
    
    // Endpoint for database restore
    [HttpPost("restore")]
    [SwaggerOperation("Restore database")]
    [RoleVerification(Roles.DatabaseAdmin)]
    public async Task<IActionResult> Restore([FromBody] string backupFilePath)
    {
        await _adminService.RestoreData(backupFilePath);
        return Ok(new { message = "Database restore successfully done" });
    }
    
    // Endpoint for settring user role
    [HttpPatch("role")]
    [SwaggerOperation("Set user role")]
    [RoleVerification(Roles.PlatformAdmin)]
    public async Task<IActionResult> SetRole(SetRoleRequest request)
    {
        var user = await _adminService.SetRole(request.UserEmail, request.Role);
        return Ok(user);
    }
    
    // Endpoint for connecting user to device
    [HttpPatch("connect")]
    [SwaggerOperation("Connect user to device")]
    [RoleVerification(Roles.HospitalAdmin)]
    public async Task<IActionResult> ConnectUserToDevice(ConnectUserToDeviceRequest request)
    {
        var user = await _adminService.ConnectUserToDevice(request.UserEmail, request.RoomName);
        return Ok(user);
    }
    
    // Endpoint for retrieving a contact by its ID
    [HttpGet("requestLog/{requestLogId:Guid}")]
    [SwaggerOperation("Get request log by id")]
    [RoleVerification(Roles.SystemAdmin)]
    public async Task<IActionResult> GetRequestLog([FromRoute] Guid requestLogId)
    {
        var requestLog = await _requestLogService.GetById(requestLogId);
        return Ok(requestLog);
    }
    
    // Endpoint for retrieving all contacts
    [HttpGet("requestLog")]
    [SwaggerOperation("Get all request logs")]
    [RoleVerification(Roles.SystemAdmin)]
    public async Task<IActionResult> GetAllRequestLogs()
    {
        var requestLogs = await _requestLogService.GetAll();
        return Ok(requestLogs);
    }
    
    // Endpoint for deleting a contact by its ID
    [HttpDelete("requestLog/{requestLogId:Guid}")]
    [SwaggerOperation("Delete request log by id")]
    [RoleVerification(Roles.SystemAdmin)]
    public async Task<IActionResult> DeleteRequestLog([FromRoute] Guid requestLogId)
    {
        var requestLog = await _requestLogService.Delete(requestLogId);
        return Ok(requestLog);
    }
    
    // Endpoint for generating a report on contacts for a specified user and time period
    [HttpGet("requestLog/report")]
    [SwaggerOperation("Get request logs report for a specific period")]
    [RoleVerification(Roles.SystemAdmin)]
    public async Task<IActionResult> GenerateRequestLogsReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var contactsReport = await _requestLogService.GetRequestLogsReport(startDate, endDate);
        return Ok(contactsReport);
    }
}
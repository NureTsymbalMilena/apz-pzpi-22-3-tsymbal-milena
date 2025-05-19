using InRoom.API.Contracts.Notification;
using InRoom.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    // Endpoint for creating a new notification for a user
    [HttpPost]
    [SwaggerOperation("Create notification")]
    public async Task<IActionResult> AddNotification(CreateNotificationRequest request)
    {
        var notification = await _notificationService.Add(request.UserId, request.Message);
        return Ok(notification);
    }
    
    // Endpoint for retrieving a notification by its ID
    [HttpGet("{notificationId:Guid}")]
    [SwaggerOperation("Get notification by id")]
    public async Task<IActionResult> GetNotification([FromRoute] Guid notificationId)
    {
        var notification = await _notificationService.GetById(notificationId);
        return Ok(notification);
    }
    
    // Endpoint for retrieving all notifications
    [HttpGet]
    [SwaggerOperation("Get all notifications")]
    public async Task<IActionResult> GetAllNotifications()
    {
        var notifications = await _notificationService.GetAll();
        return Ok(notifications);
    }
    
    // Endpoint for editing a notification by its ID
    [HttpPatch("{notificationId:Guid}")]
    [SwaggerOperation("Edit notification by id")]
    public async Task<IActionResult> EditNotification([FromRoute] Guid notificationId, UpdateNotificationRequest request)
    {
        var notification = await _notificationService.Update(notificationId, request.Message);
        return Ok(notification);
    }
    
    // Endpoint for deleting a notification by its ID
    [HttpDelete("{notificationId:Guid}")]
    [SwaggerOperation("Delete notification by id")]
    public async Task<IActionResult> DeleteNotification([FromRoute] Guid notificationId)
    {
        var notification = await _notificationService.Delete(notificationId);
        return Ok(notification);
    }
    
    // Endpoint for sending a notification when a user exits a defined zone
    [HttpPost("zone-exit/{userId:Guid}")]
    [SwaggerOperation("Send notification on zone exit")]
    public async Task<IActionResult> SendZoneExitNotification([FromRoute] Guid userId)
    {
        return Ok(new { Message = $"Notification sent to administration" });
    }
}

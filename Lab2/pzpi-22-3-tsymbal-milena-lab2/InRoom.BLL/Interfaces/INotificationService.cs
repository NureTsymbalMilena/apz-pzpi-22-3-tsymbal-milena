using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface INotificationService: IGenericService<Notification>
{
    Task<Notification> Add(Guid userId, string message);
    Task<Notification> Update(Guid notificationId, string message);
    Task AddRange(Device device, User user);
}
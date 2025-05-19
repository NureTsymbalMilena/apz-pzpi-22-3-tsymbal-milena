using InRoom.DLL.Models;

namespace InRoom.DAL.Interfaces;

public interface INotificationRepository: IRepository<Notification>
{
    Task AddRange(List<Notification> notifications);
}
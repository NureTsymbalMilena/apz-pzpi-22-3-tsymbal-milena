using InRoom.DLL.Models;

namespace InRoom.DAL.Interfaces;

public interface IMovementRepository: IRepository<Movement>
{
    Task<List<Movement>> GetMovementsByDateRangeAndUserId(Guid userId, DateTime startDate, DateTime endDate);
    Task<List<Movement>> GetMovementsByDeviceId(Guid deviceId);
}
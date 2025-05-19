using InRoom.DLL.Models;

namespace InRoom.DAL.Interfaces;

public interface IRequestLogRepository: IRepository<RequestLog>
{
    Task AddRange(List<RequestLog> requestLogs);
    Task<List<RequestLog>> GetRequestLogsByDateRange(DateTime startDate, DateTime endDate);
}
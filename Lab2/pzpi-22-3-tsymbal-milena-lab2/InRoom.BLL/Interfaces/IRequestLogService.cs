using InRoom.BLL.Contracts.User;
using InRoom.DAL;
using InRoom.DLL.Models;
using Microsoft.AspNetCore.Http;

namespace InRoom.BLL.Interfaces;

public interface IRequestLogService: IGenericService<RequestLog>
{
    Task LogAsync(HttpRequest request, HttpResponse response);
    Task<RequestLogsReportResponse> GetRequestLogsReport(DateTime startDate, DateTime endDate);
}
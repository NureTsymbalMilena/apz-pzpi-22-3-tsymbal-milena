namespace InRoom.BLL.Contracts.User;
using InRoom.DLL.Models;

public class RequestLogsReportResponse
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<RequestLog>? RequestLogs { get; set; }
}
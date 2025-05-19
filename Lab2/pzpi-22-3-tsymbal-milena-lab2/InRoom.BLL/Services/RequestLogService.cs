using InRoom.BLL.Contracts.User;
using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.AspNetCore.Http;

namespace InRoom.BLL.Services;

public class RequestLogService: GenericService<RequestLog>, IRequestLogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRequestLogRepository _requestLogRepository;

    // Constructor to inject the required RequestLogRepository dependencies
    public RequestLogService(IUnitOfWork unitOfWork, IRequestLogRepository requestLogRepository): base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _requestLogRepository = requestLogRepository;
    }

    // Method to add a new log
    public async Task LogAsync(HttpRequest request, HttpResponse response)
    {
        var log = new RequestLog
        {
            Method = request.Method,
            Path = request.Path,
            StatusCode = response.StatusCode,
            Timestamp = DateTime.UtcNow
        };

        await _requestLogRepository.Add(log);
    }
    
    // Method to get movements report
    public async Task<RequestLogsReportResponse> GetRequestLogsReport(DateTime startDate, DateTime endDate)
    {

        var requestLogs = await _requestLogRepository.GetRequestLogsByDateRange(startDate, endDate);
        
        var requestLogsReportResponse = new RequestLogsReportResponse()
        {
            StartDate = startDate,
            EndDate = endDate,
            RequestLogs = requestLogs
        };
        
        return requestLogsReportResponse;
    }
}
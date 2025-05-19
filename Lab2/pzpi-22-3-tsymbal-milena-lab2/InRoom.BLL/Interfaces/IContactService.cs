using InRoom.BLL.Contracts.User;
using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface IContactService: IGenericService<Contact>
{
    Task<Contact> Add(Guid initiatorId, Guid receiverId, Guid deviceId,
        float initiatorX, float initiatorY, float initiatorZ, 
        float receiverX, float receiverY, float receiverZ);
    Task<Contact> Update(Guid contactId, float distance);
    Task<ContacsReportResponse> GetContactsReport(Guid userId, DateTime startDate, DateTime endDate);
    Task<EpidemiologicalRiskAnalysisResponse> AnalyzeEpidemiologicalRisk(Guid userId);
}
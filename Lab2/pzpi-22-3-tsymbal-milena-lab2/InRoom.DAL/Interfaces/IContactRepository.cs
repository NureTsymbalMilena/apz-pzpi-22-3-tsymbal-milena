using InRoom.DLL.Models;

namespace InRoom.DAL.Interfaces;

public interface IContactRepository: IRepository<Contact>
{
    Task<List<Contact>> GetContactsByDateRangeAndUserId(Guid userId, DateTime startDate, DateTime endDate);
    Task<List<Contact>> GetByUserIdAndDaysNumber(Guid userId, int daysAgo = 14);
}
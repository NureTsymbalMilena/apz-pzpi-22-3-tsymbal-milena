namespace InRoom.BLL.Contracts.User;
using InRoom.DLL.Models;

public class ContacsReportResponse
{
    public User User { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Contact>? Contacts { get; set; }
}
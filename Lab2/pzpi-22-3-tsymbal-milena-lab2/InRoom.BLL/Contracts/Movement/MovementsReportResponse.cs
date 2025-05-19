namespace InRoom.BLL.Contracts.Movement;
using InRoom.DLL.Models;

public class MovementsReportResponse
{
    public User User { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Movement>? Movements { get; set; }
}
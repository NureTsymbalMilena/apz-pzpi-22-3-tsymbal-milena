namespace InRoom.BLL.Contracts.User;
using InRoom.DLL.Models;

public class EpidemiologicalRiskAnalysisResponse
{
    public User User { get; set; }

    public List<Contact> Contacts { get; set; } 

    public double TotalRisk { get; set; } 

    public Disease UserDisease { get; set; }

    public bool IsContagious { get; set; }

    public double TotalContagionRate { get; set; }

    public TimeSpan AverageContactDuration { get; set; }

    public string RiskLevel { get; set; }
    public  List<Disease> PotentialDiseases { get; set; }
}
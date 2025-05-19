using InRoom.DLL.Enums;

namespace InRoom.DLL.Models;

public class Disease
{
    public Guid DiseaseId { get; set; }
    public string Name { get; set; }
    public SeverityLevel SeverityLevel { get; set; }
    public bool Contagious { get; set; }
    public double ContagionRate { get; set; }
    public int IncubationPeriod { get; set; }
    public double MortalityRate { get; set; }
    public TransmissionMode TransmissionMode { get; set; }
}
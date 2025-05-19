using System.ComponentModel.DataAnnotations;
using InRoom.DLL.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Contracts.Disease;

public class CreateDiseaseRequest
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public SeverityLevel SeverityLevel { get; set; }
    
    [Required]
    public bool Contagious { get; set; }
    
    [Required]
    public double ContagionRate { get; set; }
    
    [Required]
    public int IncubationPeriod { get; set; }
    
    [Required]
    public double MortalityRate { get; set; }
    
    [Required]
    public TransmissionMode TransmissionMode { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace InRoom.API.Contracts.Hospital;

public class CreateHospitalRequest
{
    [Required]
    public string Name { get; set; }
        
    [Required]
    public string Address { get; set; }
}
using InRoom.DLL.Enums;
using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface IDiseaseService: IGenericService<Disease>
{
    Task<Disease> Add(
        string diseaseName, 
        SeverityLevel severityLevel, 
        bool contagious, 
        double contagionRate, 
        int incubationPeriod, 
        double mortalityRate, 
        TransmissionMode transmissionMode);
    
    Task<Disease> Update(
        Guid diseaseId, 
        string diseaseName, 
        SeverityLevel severityLevel, 
        bool contagious, 
        double contagionRate, 
        int incubationPeriod, 
        double mortalityRate, 
        TransmissionMode transmissionMode);

    new Task<Guid> Delete(Guid diseaseId);
}
using InRoom.DLL.Models;

namespace InRoom.DAL.Interfaces;

public interface IDiseaseRepository: IRepository<Disease>
{
    Task<Disease?> GetByName(string diseaseName);
}
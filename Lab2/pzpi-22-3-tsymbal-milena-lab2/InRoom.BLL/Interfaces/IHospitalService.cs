using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface IHospitalService: IGenericService<Hospital>
{
    Task<Hospital> Add(string hospitalName, string address);
    Task<Hospital> Update(Guid hospitalId, string hospitalName, string address);
}
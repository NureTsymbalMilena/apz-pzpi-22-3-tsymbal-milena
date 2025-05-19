using InRoom.DLL.Enums;
using InRoom.DLL.Models;

namespace InRoom.BLL.Interfaces;

public interface IAdminService: IGenericService<User>
{
    Task<string> BackupData(string? outputDirectory);
    Task RestoreData(string backupFilePath);
    Task<User> ConnectUserToDevice(string userEmail, string roomName);
    Task<User> SetRole(string userEmail, Roles role);
}
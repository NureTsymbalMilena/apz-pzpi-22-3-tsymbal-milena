using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Enums;
using Microsoft.Data.SqlClient;
using User = InRoom.DLL.Models.User;

namespace InRoom.BLL.Services;

public class AdminService: GenericService<User>, IAdminService
{
    private readonly string _connectionString;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IDeviceRepository _deviceRepository;

    public AdminService(string? connectionString, IUnitOfWork unitOfWork, IUserRepository userRepository, IDeviceRepository deviceRepository) : base(unitOfWork)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _deviceRepository = deviceRepository;
    }

    public async Task<string> BackupData(string? outputDirectory)
    {
        try
        {
            outputDirectory ??= @"C:\Backups"; 
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string backupFileName = $"backup_{DateTime.Now:yyyyMMddHHmmss}.bak";
            string backupFilePath = Path.Combine(outputDirectory, backupFileName);

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                SqlCommand com = new SqlCommand($"BACKUP DATABASE InRoom TO DISK = @BackupFilePath", con);
                com.Parameters.AddWithValue("@BackupFilePath", backupFilePath);

                await com.ExecuteNonQueryAsync(); 
                con.Close();
            }

            return backupFilePath;
        }
        catch (Exception ex)
        {
            throw new ApiException($"An error occurred while creating the backup: {ex.Message}", 500);
        }
    }
    
    public async Task RestoreData(string backupFilePath)
    {
        try
        {
            if (string.IsNullOrEmpty(backupFilePath))
            {
                throw new ArgumentException("Backup file path is required.");
            }

            string restoreQuery = $@"
            USE master;

            -- Закрываем все соединения с базой данных InRoom
            ALTER DATABASE InRoom SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

            -- Выполняем восстановление базы данных
            RESTORE DATABASE InRoom FROM DISK = @BackupFilePath WITH REPLACE;

            -- Возвращаем базу данных в многопользовательский режим
            ALTER DATABASE InRoom SET MULTI_USER;
        ";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                SqlCommand com = new SqlCommand(restoreQuery, con);
                com.Parameters.AddWithValue("@BackupFilePath", backupFilePath);

                await com.ExecuteNonQueryAsync();
                con.Close();
            }

        }
        catch (Exception ex)
        {
            throw new ApiException($"An error occurred while restoring the backup: {ex.Message}", 500);
        }
    }

    public async Task<User> ConnectUserToDevice(string userEmail, string roomName)
    {
        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ApiException($"User with email {userEmail} not found", 404);
        }
        
        var device = await _deviceRepository.GetByRoomName(roomName);
        if (device == null)
        {
            throw new ApiException($"Device connected to room {roomName} not found", 404);
        }
        
        user.DeviceId = device.DeviceId;
        user.Device = device;
        
        await _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return user;
    }
    
    public async Task<User> SetRole(string userEmail, Roles role)
    {
        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ApiException($"User with email {userEmail} not found", 404);
        }
        
        if (!Enum.IsDefined(typeof(Roles), role))
        {
            throw new ApiException("Invalid role", 400);
        }
        
        user.Role = role;
        
        await _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return user;
    }
}
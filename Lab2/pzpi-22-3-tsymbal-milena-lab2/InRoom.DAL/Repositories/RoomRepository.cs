using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    private readonly ApplicationDbContext _context;

    // Constructor that initializes the repository with the ApplicationDbContext
    public RoomRepository(ApplicationDbContext context) : base(context)
    {
        _context = context; // Initializes the context for the repository
    }

    // Method to retrieve a room by its name
    public async Task<Room?> GetByName(string roomName)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(room => room.Name == roomName);
        return room;
    }
}

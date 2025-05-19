using InRoom.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    // Constructor that initializes the repository with a DbContext
    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    // Method to retrieve an entity by its ID
    public async Task<TEntity> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Method to retrieve all entities of the specified type
    public async Task<List<TEntity>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    // Method to add a new entity to the database
    public async Task Add(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    // Method to update an existing entity in the database
    public async Task Update(TEntity entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    // Method to delete an entity by its ID
    public async Task Delete(Guid id)
    {
        var entity = await GetById(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }
}

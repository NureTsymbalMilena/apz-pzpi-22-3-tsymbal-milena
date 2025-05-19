using InRoom.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context; 
    private readonly Dictionary<Type, object> _repositories = new();

    // Constructor to initialize the UnitOfWork with a provided ApplicationDbContext
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    // Method to retrieve a repository for a given entity type
    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (!_repositories.ContainsKey(typeof(TEntity)))
        {
            var repositoryInstance = new GenericRepository<TEntity>(_context);
            _repositories[typeof(TEntity)] = repositoryInstance;
        }

        return (IRepository<TEntity>)_repositories[typeof(TEntity)];
    }

    // Method to save changes made in the context asynchronously
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // Dispose method to clean up the resources, primarily the database context
    public void Dispose()
    {
        _context.Dispose();
    }
}

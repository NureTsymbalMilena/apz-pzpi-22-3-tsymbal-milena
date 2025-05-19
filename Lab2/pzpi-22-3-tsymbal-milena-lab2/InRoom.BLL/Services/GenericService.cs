using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;

namespace InRoom.BLL.Services;

public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
{
    private readonly IUnitOfWork _unitOfWork;
    protected IRepository<TEntity> Repository => _unitOfWork.Repository<TEntity>();

    // Constructor to inject the required UnitOfWork dependency
    public GenericService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
   
    // Method to delete an entity by its ID
    public async Task<Guid> Delete(Guid entityId)
    {
        var entity = await Repository.GetById(entityId);

        if (entity == null)
        {
            throw new ApiException("Entity wasn't found", 404);
        }

        await Repository.Delete(entityId);

        await _unitOfWork.SaveChangesAsync();

        return entityId;
    }

    // Method to retrieve an entity by its ID
    public async Task<TEntity> GetById(Guid entityId)
    {
        var entity = await Repository.GetById(entityId);

        if (entity == null)
        {
            throw new ApiException("Entity wasn't found", 404);
        }

        return entity;
    }

    // Method to retrieve all entities
    public Task<List<TEntity>> GetAll()
    {
        return Repository.GetAll();
    }
}




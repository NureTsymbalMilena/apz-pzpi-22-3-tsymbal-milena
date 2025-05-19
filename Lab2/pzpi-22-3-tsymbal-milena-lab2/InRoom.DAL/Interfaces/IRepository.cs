namespace InRoom.DAL.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetById(Guid id);
    Task<List<TEntity>> GetAll();
    Task Add(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(Guid id);
}
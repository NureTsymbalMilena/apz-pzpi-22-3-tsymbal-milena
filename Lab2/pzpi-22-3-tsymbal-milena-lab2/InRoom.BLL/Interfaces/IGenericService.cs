namespace InRoom.BLL.Interfaces;

public interface IGenericService<TEntity> where TEntity : class
{
    Task<Guid> Delete(Guid entityId);
    Task<TEntity> GetById(Guid entityId);
    Task<List<TEntity>> GetAll();
}
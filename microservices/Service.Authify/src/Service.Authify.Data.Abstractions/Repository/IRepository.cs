namespace Service.Authify.Data.Repository;

public interface IRepository<TEntity>
{
    Task<ICollection<TEntity>> Get(CancellationToken cancellationToken = default);
    Task Create(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> GetOneById(Guid id, CancellationToken cancellationToken = default);
    Task Update(TEntity entity, CancellationToken cancellationToken = default);
    Task Delete(Guid id, CancellationToken cancellationToken = default);
}
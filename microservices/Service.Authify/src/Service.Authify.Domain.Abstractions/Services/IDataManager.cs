namespace Service.Authify.Domain.Services;

public interface IDataManager<TEntity>
{
    Task Create(TEntity entity, CancellationToken cancellationToken = default);

    Task Update(TEntity entity, CancellationToken cancellationToken = default);
}
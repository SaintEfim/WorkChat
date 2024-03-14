namespace Service.Authify.Domain.Services;

public interface IDataProvider<TEntity>
{
    Task<ICollection<TEntity>> Get(
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetOneById(Guid id, CancellationToken cancellationToken = default);
}
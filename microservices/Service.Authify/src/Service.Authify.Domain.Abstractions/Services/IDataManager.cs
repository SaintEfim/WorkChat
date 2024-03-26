namespace Service.Authify.Domain.Services;

public interface IDataManager<in TModel>
{
    Task Create(TModel entity, CancellationToken cancellationToken = default);

    Task Update(TModel entity, CancellationToken cancellationToken = default);

    Task Delete(Guid id, CancellationToken cancellationToken = default);
}
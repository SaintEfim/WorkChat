namespace Service.Authify.Domain.Services;

public interface IDataProvider<TModel>
{
    Task<ICollection<TModel>> Get(
        CancellationToken cancellationToken = default);

    Task<TModel?> GetOneById(Guid id, CancellationToken cancellationToken = default);
}
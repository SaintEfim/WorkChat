using AutoMapper;
using Service.Authify.Data.Repository;

namespace Service.Authify.Domain.Services;

public abstract class DataProviderBase<TRepository, TModel> : IDataProvider<TModel>
    where TRepository : IRepository<TModel>
{
    protected DataProviderBase(
        IMapper mapper,
        TRepository repository
    )
    {
        Mapper = mapper;
        Repository = repository;
    }

    private IMapper Mapper { get; }
    private TRepository Repository { get; }

    public virtual async Task<ICollection<TModel>> Get(CancellationToken cancellationToken = default)
    {
        return await Repository.Get(cancellationToken);
    }

    public async Task<TModel?> GetOneById(Guid id, CancellationToken cancellationToken = default)
    {
        return await Repository.GetOneById(id, cancellationToken);
    }
}
using AutoMapper;
using Service.Authify.Data.Repository;

namespace Service.Authify.Domain.Services;

public abstract class DataProviderBase<TRepository, TEntity> : IDataProvider<TEntity>
    where TRepository : IRepository<TEntity>
{
    protected DataProviderBase(
        IMapper mapper,
        TRepository repository
    )
    {
        this.Mapper = mapper;
        this.Repository = repository;
    }

    protected IMapper Mapper { get; }
    protected TRepository Repository { get; }

    public async Task<ICollection<TEntity>> Get(CancellationToken cancellationToken = default)
    {
        return await Repository.Get(cancellationToken);
    }

    public async Task<TEntity?> GetOneById(Guid id, CancellationToken cancellationToken = default)
    {
        return await Repository.GetOneById(id, cancellationToken);
    }
}
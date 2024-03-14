using AutoMapper;
using Service.Authify.Data.Repository;

namespace Service.Authify.Domain.Services;

public abstract class DataManagerBase<TRepository, TEntity> : IDataManager<TEntity>
    where TRepository : IRepository<TEntity>
{
    protected DataManagerBase(
        IMapper mapper,
        TRepository repository
    )
    {
        this.Mapper = mapper;
        this.Repository = repository;
    }

    protected IMapper Mapper { get; }
    protected TRepository Repository { get; }

    public async Task Create(TEntity entity, CancellationToken cancellationToken = default)
    {
        await this.Repository.Create(entity, cancellationToken);
    }

    public async Task Update(TEntity entity, CancellationToken cancellationToken = default)
    {
        await this.Repository.Update(entity, cancellationToken);
    }
}
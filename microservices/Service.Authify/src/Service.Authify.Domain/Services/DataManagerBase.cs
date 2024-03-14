using AutoMapper;
using Service.Authify.Data.Repository;

namespace Service.Authify.Domain.Services;

public abstract class DataManagerBase<TRepository, TModel> : IDataManager<TModel>
    where TRepository : IRepository<TModel>
{
    protected DataManagerBase(
        IMapper mapper,
        TRepository repository
    )
    {
        Mapper = mapper;
        Repository = repository;
    }

    protected IMapper Mapper { get; }
    protected TRepository Repository { get; }

    public virtual async Task Create(TModel entity, CancellationToken cancellationToken = default)
    {
        await Repository.Create(entity, cancellationToken);
    }

    public async Task Update(TModel entity, CancellationToken cancellationToken = default)
    {
        await Repository.Update(entity, cancellationToken);
    }

    public Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        return Repository.Delete(id, cancellationToken);
    }
}
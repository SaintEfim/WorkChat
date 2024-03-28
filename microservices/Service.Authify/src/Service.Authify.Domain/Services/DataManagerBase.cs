using AutoMapper;
using Microsoft.Extensions.Logging;
using Service.Authify.Data.Repository;

namespace Service.Authify.Domain.Services;

public abstract class DataManagerBase<TManager, TRepository, TDomain> : IDataManager<TDomain>
    where TManager : IDataManager<TDomain>
    where TRepository : IRepository<TDomain>
{
    protected DataManagerBase(
        IMapper mapper,
        ILogger<TManager> logger,
        TRepository repository
    )
    {
        Mapper = mapper;
        Logger = logger;
        Repository = repository;
    }

    protected IMapper Mapper { get; }

    protected TRepository Repository { get; }

    private ILogger<TManager> Logger { get; }

    public virtual async Task Create(TDomain entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await Repository.Create(entity, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating entity: {Message}", ex.Message);
            throw;
        }
    }

    public async Task Update(TDomain entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await Repository.Update(entity, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating entity: {Message}", ex.Message);
            throw;
        }
    }

    public Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(id);

            return Repository.Delete(id, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when deleting an entity: {Message}", ex.Message);
            throw;
        }
    }
}
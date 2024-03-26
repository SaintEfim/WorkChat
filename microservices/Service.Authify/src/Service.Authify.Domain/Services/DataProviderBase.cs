using AutoMapper;
using Microsoft.Extensions.Logging;
using Service.Authify.Data.Repository;

namespace Service.Authify.Domain.Services;

public abstract class DataProviderBase<TProvider, TRepository, TDomain> : IDataProvider<TDomain>
    where TProvider : IDataProvider<TDomain>
    where TRepository : IRepository<TDomain>
{
    protected DataProviderBase(
        IMapper mapper,
        ILogger<TProvider> logger,
        TRepository repository
    )
    {
        Mapper = mapper;
        Logger = logger;
        Repository = repository;
    }

    protected IMapper Mapper { get; }

    private TRepository Repository { get; }

    private ILogger<TProvider> Logger { get; }

    public virtual async Task<ICollection<TDomain>> Get(CancellationToken cancellationToken = default)
    {
        try
        {
            return await Repository.Get(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error while receiving data: {ex.Message}");
            throw;
        }
    }

    public async Task<TDomain?> GetOneById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await Repository.GetOneById(id, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error when retrieving data by ID: {ex.Message}");
            throw;
        }
    }
}
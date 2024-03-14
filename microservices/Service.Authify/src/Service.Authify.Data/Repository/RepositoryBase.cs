using Microsoft.EntityFrameworkCore;

namespace Service.Authify.Data.Repository;

public abstract class RepositoryBase<TDbContext, TEntity> : IRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : class
{
    protected RepositoryBase(TDbContext context)
    {
        Context = context;
    }

    private TDbContext Context { get; }

    public virtual async Task<ICollection<TEntity>> Get(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public virtual async Task Create(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetOneById(Guid id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await Context.Set<TEntity>().FindAsync([id], cancellationToken);
    }

    public virtual async Task Update(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        Context.Set<TEntity>().Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var entityToDelete = await Context.Set<TEntity>().FindAsync([id], cancellationToken);

        if (entityToDelete != null)
        {
            Context.Set<TEntity>().Remove(entityToDelete);
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Service.Authify.Domain.Models;

namespace Service.Authify.Data.Repository;

public class UserCredentialRepository<TDbContext> : IUserCredentialRepository
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    protected UserCredentialRepository(TDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<UserCredential>> Get(CancellationToken cancellationToken = default)
    {
        return await _context.Set<UserCredential>().ToListAsync(cancellationToken);
    }

    public async Task Create(UserCredential entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _context.Set<UserCredential>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserCredential?> GetOneById(Guid id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await _context.Set<UserCredential>().FindAsync([id], cancellationToken);
    }

    public async Task Update(UserCredential entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<UserCredential>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
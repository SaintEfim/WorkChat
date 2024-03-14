using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Authify.Domain.Models;

namespace Service.Authify.Data.Repository;

public class UserCredentialRepository<TDbContext> :
    RepositoryBase<UserCredentialRepository<TDbContext>, TDbContext, UserCredential>,
    IUserCredentialRepository
    where TDbContext : DbContext
{
    public UserCredentialRepository(TDbContext context, ILogger<UserCredentialRepository<TDbContext>> logger) : base(
        context, logger)
    {
    }
}
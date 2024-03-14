using Microsoft.EntityFrameworkCore;
using Service.Authify.Domain.Models;

namespace Service.Authify.Data.Repository;

public class UserCredentialRepository<TDbContext> : RepositoryBase<TDbContext, UserCredential>,
    IUserCredentialRepository
    where TDbContext : DbContext
{
    public UserCredentialRepository(TDbContext context) : base(context)
    {
    }
}
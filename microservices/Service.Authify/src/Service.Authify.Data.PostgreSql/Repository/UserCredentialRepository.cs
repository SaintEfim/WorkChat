using Microsoft.Extensions.Logging;
using Service.Authify.Data.PostgreSql.Context;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Models;

namespace Service.Authify.Data.PostgreSql.Repository;

public class UserCredentialRepository : UserCredentialRepository<UserCredentialDbContext>
{
    public UserCredentialRepository(UserCredentialDbContext context, ILogger<UserCredentialRepository> logger) : base(context, logger)
    {
    }
}
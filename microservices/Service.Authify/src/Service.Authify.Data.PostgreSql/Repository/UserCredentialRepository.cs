using Service.Authify.Data.PostgreSql.Context;
using Service.Authify.Data.Repository;

namespace Service.Authify.Data.PostgreSql.Repository;

public class UserCredentialRepository : UserCredentialRepository<UserCredentialDbContext>
{
    public UserCredentialRepository(UserCredentialDbContext context) : base(context)
    {
    }
}
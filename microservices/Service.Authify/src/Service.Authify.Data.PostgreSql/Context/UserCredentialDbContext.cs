using Microsoft.EntityFrameworkCore;
using Service.Authify.Domain.Models;

namespace Service.Authify.Data.PostgreSql.Context;

public sealed class UserCredentialDbContext : DbContext
{
    public UserCredentialDbContext(DbContextOptions<UserCredentialDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    public DbSet<UserCredential> UsersCredentials { get; set; } = null!;
}
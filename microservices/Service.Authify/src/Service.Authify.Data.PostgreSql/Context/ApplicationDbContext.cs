using Microsoft.EntityFrameworkCore;
using Service.Authify.Domain.Models;

namespace Service.Authify.Data.PostgreSql.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    public DbSet<UserCredential> UsersCredentials { get; set; }
}
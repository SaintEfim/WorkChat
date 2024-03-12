using Microsoft.EntityFrameworkCore;
using Service.Authify.Data.Exceptions;
using Service.Authify.Data.PostgreSql.Context;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Models;

namespace Service.Authify.Data.PostgreSql.Repository
{
    public class UserCredentialRepository : IUserCredentialRepository
    {
        private readonly ApplicationDbContext _context;

        public UserCredentialRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<UserCredential>> Get(CancellationToken cancellationToken = default)
        {
            return await _context.UsersCredentials.ToListAsync(cancellationToken);
        }

        public async Task Create(UserCredential user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            await _context.UsersCredentials.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserCredential> GetOneById(Guid id, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(id);

            var user = await _context.UsersCredentials
                .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundUserException($"User with id {id} not found.");
            }

            return user;
        }

        public async Task Update(UserCredential user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            _context.UsersCredentials.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
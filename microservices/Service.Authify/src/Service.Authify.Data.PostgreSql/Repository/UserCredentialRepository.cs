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
            try
            {
                return await _context.UsersCredentials.ToListAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseAccessException("Ошибка при получении данных из базы данных.", ex);
            }
        }

        public async Task Create(UserCredential user, CancellationToken cancellationToken = default)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                await _context.UsersCredentials.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseAccessException("Ошибка при сохранении данных в базе данных.", ex);
            }
        }

        public async Task<UserCredential> GetOneById(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _context.UsersCredentials
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundUserException($"User with id {id} not found.");
            }

            return user;
        }

        public async Task Update(UserCredential user, CancellationToken cancellationToken = default)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                _context.UsersCredentials.Update(user);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseAccessException("Ошибка при обновлении данных в базы данных.", ex);
            }
        }
    }
}
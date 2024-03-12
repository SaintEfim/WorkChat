using Service.Authify.Domain.Models;

namespace Service.Authify.Data.Repository;

public interface IUserCredentialRepository
{
    Task Create(
        UserCredential user,
        CancellationToken cancellationToken = default);

    Task<ICollection<UserCredential>> Get(
        CancellationToken cancellationToken = default);

    Task<UserCredential?> GetOneById(Guid id, CancellationToken cancellationToken = default);

    Task Update(UserCredential user, CancellationToken cancellationToken = default);
}
using Service.Authify.Domain.Models;

namespace Service.Authify.Domain.Services;

public interface IUserCredentialProvider
{
    Task<ICollection<UserCredential>> Get(
        CancellationToken cancellationToken = default);

    Task<UserCredential> GetUserById(Guid id, CancellationToken cancellationToken = default);
}
using Service.Authify.Domain.Models;

namespace Service.Authify.Domain.Services;

public interface IUserCredentialProvider
{
    Task<ICollection<UserCredential>> Get(
        CancellationToken cancellationToken = default);
}
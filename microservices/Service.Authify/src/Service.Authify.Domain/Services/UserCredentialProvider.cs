using Service.Authify.Data.Repository;
using Service.Authify.Domain.Models;

namespace Service.Authify.Domain.Services;

public class UserCredentialProvider : IUserCredentialProvider
{
    private readonly IUserCredentialRepository _repository;

    public UserCredentialProvider(IUserCredentialRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<UserCredential>> Get(CancellationToken cancellationToken = default)
    {
        return await _repository.Get();
    }
}
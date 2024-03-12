using Service.Authify.Data.Repository;
using Service.Authify.Domain.Exceptions;
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
        var users = await _repository.Get(cancellationToken);

        if (users is { Count: 0 })
        {
            throw new DataNotFoundException("No users found.");
        }

        return users;
    }

    public async Task<UserCredential> GetUserById(Guid id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var user = await _repository.GetOneById(id, cancellationToken);

        if (user == null)
        {
            throw new NotFoundUserException($"User with id {id} not found.");
        }

        return user;
    }
}
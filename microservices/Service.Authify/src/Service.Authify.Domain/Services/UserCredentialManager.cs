using Service.Authify.Data.Repository;
using Service.Authify.Domain.Exceptions;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.Domain.Services;

public class UserCredentialManager : IUserCredentialManager
{
    private readonly IUserCredentialRepository _repository;

    public UserCredentialManager(IUserCredentialRepository repository)
    {
        _repository = repository;
    }

    public async Task Register(RegistrationRequest registrationRequest, CancellationToken cancellationToken = default)
    {
        if (_repository.IsUniqueUser(registrationRequest.Email))
        {
            throw new DuplicateUserException(
                $"A user with the same {registrationRequest.Email} address already exists.");
        }
        
        await _repository.Register(registrationRequest, cancellationToken);
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetUserByEmailAndPasswordAsync(loginRequest, cancellationToken);
        return await _repository.Login(user, cancellationToken);
    }
    
    
}
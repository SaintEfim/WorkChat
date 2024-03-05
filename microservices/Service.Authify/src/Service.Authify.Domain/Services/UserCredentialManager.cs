using AutoMapper;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Exceptions;
using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.Domain.Services;

public class UserCredentialManager : IUserCredentialManager
{
    private readonly IUserCredentialRepository _repository;
    private readonly IMapper _mapper;

    public UserCredentialManager(IUserCredentialRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Register(RegistrationRequest registrationRequest, CancellationToken cancellationToken = default)
    {
        if (await _repository.IsUniqueUser(registrationRequest.Email, cancellationToken))
        {
            throw new DuplicateUserException(
                $"A user with the same {registrationRequest.Email} address already exists.");
        }

        var user = _mapper.Map<UserCredential>(registrationRequest);

        if (user == null)
        {
            throw new MappingFailedException("Failed to map RegistrationRequest to UserCredential.");
        }

        await _repository.Register(user, cancellationToken);
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetUserByEmailAndPassword(loginRequest, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        var loginResponse = await _repository.Login(user, cancellationToken);
        if (loginResponse == null)
        {
            throw new DataNotFoundException("Login response is null.");
        }

        return loginResponse;
    }

    public async Task<LoginResponse> Refresh(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentException("Refresh token is null or empty.", nameof(refreshToken));
        }

        var loginResponse = await _repository.Refresh(refreshToken, cancellationToken);
        if (loginResponse == null)
        {
            throw new DataNotFoundException("Login response is null.");
        }

        return loginResponse;
    }

    public async Task UpdateUser(UserCredential user, CancellationToken cancellationToken = default)
    {
        await _repository.Update(user, cancellationToken);
    }
}
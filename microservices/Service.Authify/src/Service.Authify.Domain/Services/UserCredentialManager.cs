using System.Security.Claims;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Exceptions;
using Service.Authify.Domain.Helpers;
using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.Domain.Services;

public class UserCredentialManager : IUserCredentialManager
{
    private readonly IUserCredentialRepository _repository;
    private readonly IMapper _mapper;
    private readonly IJwtHelper _jwtHelper;
    private readonly IHashHelper _hashHelper;
    private readonly string _tokenType;
    private readonly string _accessHours;
    private readonly string _refreshHours;
    private readonly string _accessSecretKey;
    private readonly string _refreshSecretKey;

    public UserCredentialManager(IUserCredentialRepository repository, IMapper mapper, IConfiguration config,
        IJwtHelper jwtHelper, IHashHelper hashHelper)
    {
        _repository = repository;
        _mapper = mapper;
        _jwtHelper = jwtHelper;
        _hashHelper = hashHelper;
        _tokenType = config.GetValue<string>("TokenType")!;
        _accessHours = config.GetValue<string>("HoursSettings:AccessHours")!;
        _refreshHours = config.GetValue<string>("HoursSettings:RefreshHours")!;
        _accessSecretKey = config.GetValue<string>("ApiSettings:AccessSecret")!;
        _refreshSecretKey = config.GetValue<string>("ApiSettings:RefreshSecret")!;
    }

    public async Task Register(RegistrationRequest registrationRequest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(registrationRequest);

        if (await IsUniqueUser(registrationRequest.Email, cancellationToken))
        {
            throw new DuplicateUserException(
                $"A user with the same {registrationRequest.Email} address already exists.");
        }

        var user = _mapper.Map<UserCredential>(registrationRequest);

        if (user == null)
        {
            throw new MappingFailedException("Failed to map RegistrationRequest to UserCredential.");
        }

        user.Email = _hashHelper.Hash(user.Email);
        user.Password = _hashHelper.Hash(user.Password);
        user.CreatedAt = DateTime.UtcNow;

        await _repository.Create(user, cancellationToken);
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(loginRequest);

        var user = await GetUserByEmailAndPassword(loginRequest, cancellationToken);

        if (user == null)
        {
            throw new NotFoundUserException($"User with email {loginRequest.Email} not found.");
        }

        if (!_hashHelper.Verify(loginRequest.Email, user.Email) &&
            !_hashHelper.Verify(loginRequest.Password, user.Password))
        {
            throw new AuthenticationFailedException("Invalid email or password.");
        }

        var accessToken = await
            _jwtHelper.GenerateToken(user.Id.ToString(), user.Role, _accessSecretKey,
                TimeSpan.Parse(_accessHours), cancellationToken);
        var refreshToken = await
            _jwtHelper.GenerateToken(user.Id.ToString(), user.Role, _refreshSecretKey,
                TimeSpan.Parse(_refreshHours), cancellationToken);

        return new LoginResponse
        {
            TokenType = _tokenType,
            AccessToken = accessToken,
            ExpiresIn = (int)TimeSpan.Parse(_accessHours).TotalSeconds,
            RefreshToken = refreshToken
        };
    }

    public async Task<LoginResponse> Refresh(string refreshToken, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);

        var (userId, userRole) = await DecodeRefreshToken(refreshToken, cancellationToken);

        var newAccessToken = await
            _jwtHelper.GenerateToken(userId, userRole, _accessSecretKey,
                TimeSpan.Parse(_accessHours), cancellationToken);
        var newRefreshToken = await
            _jwtHelper.GenerateToken(userId, userRole, _refreshSecretKey,
                TimeSpan.Parse(_refreshHours), cancellationToken);

        return new LoginResponse
        {
            TokenType = _tokenType,
            AccessToken = newAccessToken,
            ExpiresIn = (int)TimeSpan.Parse(_accessHours).TotalSeconds,
            RefreshToken = newRefreshToken
        };
    }

    public async Task UpdateUser(UserCredential user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _repository.Update(user, cancellationToken);
    }

    private async Task<(string UserId, string UserRole)> DecodeRefreshToken(string refreshToken,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);

        var user = await _jwtHelper.DecodeToken(refreshToken, _refreshSecretKey, cancellationToken);
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

        if (userId == null || userRole == null)
        {
            throw new InvalidTokenException("Invalid token. Missing required claims.");
        }

        return (userId, userRole);
    }

    private async Task<bool> IsUniqueUser(string email, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var userExists = await _repository.Get(cancellationToken);
        var result = userExists.SingleOrDefault(u => _hashHelper.Verify(email, u.Email));

        return result != null;
    }

    private async Task<UserCredential?> GetUserByEmailAndPassword(LoginRequest loginRequest,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(loginRequest);

        var userExists = await _repository.Get(cancellationToken);
        var user = userExists.SingleOrDefault(u => _hashHelper.Verify(loginRequest.Email, u.Email));

        if (user == null)
        {
            throw new NotFoundUserException($"User with email {loginRequest.Email} not found.");
        }

        return user;
    }
}
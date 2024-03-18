using System.Security.Claims;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Exceptions;
using Service.Authify.Domain.Helpers;
using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.Domain.Services;

public class UserCredentialManager : DataManagerBase<UserCredentialManager, IUserCredentialRepository, UserCredential>,
    IUserCredentialManager
{
    private readonly IJwtHelper _jwtHelper;
    private readonly IHashHelper _hashHelper;
    private readonly string _tokenType;
    private readonly string _accessHours;
    private readonly string _refreshHours;
    private readonly string _accessSecretKey;
    private readonly string _refreshSecretKey;

    public UserCredentialManager(IMapper mapper, ILogger<UserCredentialManager> logger,
        IUserCredentialRepository repository,
        IConfiguration config,
        IJwtHelper jwtHelper, IHashHelper hashHelper) : base(mapper, logger, repository)
    {
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

        var user = Mapper.Map<UserCredential>(registrationRequest);

        if (user == null)
        {
            throw new MappingFailedException("Failed to map RegistrationRequest to UserCredential.");
        }

        user.Email = _hashHelper.Hash(user.Email);
        user.Password = _hashHelper.Hash(user.Password);
        user.CreatedAt = DateTime.UtcNow;

        await Repository.Create(user, cancellationToken);
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(loginRequest);

        var user = await GetUserByEmail(loginRequest.Email, cancellationToken);

        if (user == null)
        {
            throw new NotFoundUserException($"User with email {loginRequest.Email} not found.");
        }

        if (!_hashHelper.Verify(loginRequest.Password, user.Password))
        {
            throw new AuthenticationFailedException("Invalid password.");
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

    public async Task ResetPassword(ResetPassword resetPassword, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(resetPassword);

        var oldUser = await GetUserByEmail(resetPassword.Email, cancellationToken);

        if (oldUser == null)
        {
            throw new NotFoundUserException($"User with email {resetPassword.Email} not found.");
        }

        if (!_hashHelper.Verify(resetPassword.Password, oldUser.Password))
        {
            throw new PasswordMismatchException("Passwords do not match.");
        }

        oldUser.Password = _hashHelper.Hash(resetPassword.NewPassword);
        oldUser.UpdatedAt = DateTime.UtcNow;

        await Repository.Update(oldUser, cancellationToken);
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

        var userExists = await Repository.Get(cancellationToken);
        var result = userExists.SingleOrDefault(u => _hashHelper.Verify(email, u.Email));

        return result != null;
    }

    private async Task<UserCredential?> GetUserByEmail(string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);

        var userExists = await Repository.Get(cancellationToken);
        var user = userExists.SingleOrDefault(u => _hashHelper.Verify(email, u.Email));

        return user;
    }
}
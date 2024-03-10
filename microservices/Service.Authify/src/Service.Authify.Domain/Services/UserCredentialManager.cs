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
    private readonly GenerateTokenHelper _generateToken;
    private readonly string _tokenType;
    private readonly string _accessHours;
    private readonly string _refreshHours;
    private readonly string _accessSecretKey;
    private readonly string _refreshSecretKey;

    public UserCredentialManager(IUserCredentialRepository repository, IMapper mapper,
        GenerateTokenHelper generateToken, IConfiguration config)
    {
        _repository = repository;
        _mapper = mapper;
        _generateToken = generateToken;
        _tokenType = config.GetValue<string>("TokenType")!;
        _accessHours = config.GetValue<string>("HoursSettings:AccessHours")!;
        _refreshHours = config.GetValue<string>("HoursSettings:RefreshHours")!;
        _accessSecretKey = config.GetValue<string>("ApiSettings:AccessSecret")!;
        _refreshSecretKey = config.GetValue<string>("ApiSettings:RefreshSecret")!;
    }

    public async Task Register(RegistrationRequest registrationRequest, CancellationToken cancellationToken = default)
    {
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

        user.CreatedAt = DateTime.UtcNow;
        await _repository.Create(user, cancellationToken);
    }

    public Task<LoginResponse> Login(UserCredential user, CancellationToken cancellationToken = default)
    {
        var accessToken =
            _generateToken.GenerateToken(user.Id.ToString(), user.Role, _accessSecretKey,
                TimeSpan.Parse(_accessHours));
        var refreshToken =
            _generateToken.GenerateToken(user.Id.ToString(), user.Role, _refreshSecretKey,
                TimeSpan.Parse(_refreshHours));

        return Task.FromResult(new LoginResponse
        {
            TokenType = _tokenType,
            AccessToken = accessToken,
            ExpiresIn = (int)TimeSpan.Parse(_accessHours).TotalSeconds,
            RefreshToken = refreshToken
        });
    }

    public Task<LoginResponse> Refresh(string refreshToken, CancellationToken cancellationToken = default)
    {
        var (userId, userRole) = DecodeRefreshToken(refreshToken);

        var newAccessToken =
            _generateToken.GenerateToken(userId, userRole, _accessSecretKey,
                TimeSpan.Parse(_accessHours));
        var newRefreshToken =
            _generateToken.GenerateToken(userId, userRole, _refreshSecretKey,
                TimeSpan.Parse(_refreshHours));

        return Task.FromResult(new LoginResponse
        {
            TokenType = _tokenType,
            AccessToken = newAccessToken,
            ExpiresIn = (int)TimeSpan.Parse(_accessHours).TotalSeconds,
            RefreshToken = newRefreshToken
        });
    }

    private (string UserId, string UserRole) DecodeRefreshToken(string refreshToken)
    {
        var user = DecodeJwtHelper.DecodeToken(refreshToken, _refreshSecretKey);
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
        var userExists = await _repository.Get(cancellationToken);
        var result = userExists.SingleOrDefault(u => u.Email == email);
        
        return result != null;
    }

    public async Task UpdateUser(UserCredential user, CancellationToken cancellationToken = default)
    {
        await _repository.Update(user, cancellationToken);
    }
}
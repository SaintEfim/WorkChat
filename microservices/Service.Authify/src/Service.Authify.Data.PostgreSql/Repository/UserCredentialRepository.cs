using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Authify.Data.PostgreSql.Context;
using Service.Authify.Data.Helpers;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.Data.PostgreSql.Repository;

public class UserCredentialRepository : IUserCredentialRepository
{
    private readonly ApplicationDbContext _context;
    private readonly GenerateTokenHelper _generateToken;
    private readonly string _tokenType;
    private readonly string _accessHours;
    private readonly string _refreshHours;
    private readonly string _accessSecretKey;
    private readonly string _refreshSecretKey;

    public UserCredentialRepository(ApplicationDbContext context, IMapper mapper, IConfiguration config,
        GenerateTokenHelper generateToken)
    {
        _context = context;
        _generateToken = generateToken;
        _tokenType = config.GetValue<string>("TokenType")!;
        _accessHours = config.GetValue<string>("HoursSettings:AccessHours")!;
        _refreshHours = config.GetValue<string>("HoursSettings:RefreshHours")!;
        _accessSecretKey = config.GetValue<string>("ApiSettings:AccessSecret")!;
        _refreshSecretKey = config.GetValue<string>("ApiSettings:RefreshSecret")!;
    }

    public async Task Register(UserCredential user, CancellationToken cancellationToken = default)
    {
        await _context.UsersCredentials.FromSqlRaw(
                "INSERT INTO public.\"UsersCredentials\" (Id, Email, Password, Role, CreatedAt)" +
                "VALUES ({1}, {2}, {3}, {4}, {5})", user.Id, user.Email, user.Password,
                user.Role, DateTime.UtcNow)
            .ToListAsync(cancellationToken: cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<LoginResponse> Login(UserCredential user, CancellationToken cancellationToken = default)
    {
        var accessToken =
            _generateToken.GenerateToken(user.Id.ToString(), user.Role, _accessSecretKey, TimeSpan.Parse(_accessHours));
        var refreshToken =
            _generateToken.GenerateToken(user.Id.ToString(), null, _refreshSecretKey,
                TimeSpan.Parse(_refreshHours));

        return Task.FromResult(new LoginResponse
        {
            TokenType = _tokenType,
            AccessToken = accessToken,
            ExpiresIn = (int)TimeSpan.FromHours(1).TotalSeconds,
            RefreshToken = refreshToken
        });
    }

    public async Task<ICollection<UserCredential>> Get(CancellationToken cancellationToken = default)
    {
        return await _context.UsersCredentials.FromSqlRaw("SELECT * FROM public.\"UsersCredentials\"")
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsUniqueUser(string email, CancellationToken cancellationToken = default)
    {
        var userExists = await _context.UsersCredentials
            .FromSqlInterpolated($"SELECT Email FROM public.\"UsersCredentials\" WHERE Email = {email}")
            .SingleOrDefaultAsync();

        return userExists == null;
    }

    public async Task<UserCredential> GetUserByEmailAndPasswordAsync(LoginRequest loginRequest,
        CancellationToken cancellationToken = default)
    {
        var user = await _context.UsersCredentials
            .FromSqlRaw("SELECT Email, Password FROM public.\"UsersCredentials\" WHERE Email = {1} AND Password = {2}",
                loginRequest.Email, loginRequest.Password)
            .SingleOrDefaultAsync(cancellationToken);

        return user!;
    }
}
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
    private readonly IMapper _mapper;
    private readonly GenerateTokenHelper _generateToken;
    private readonly string _accessHours;
    private readonly string _refreshHours;
    private readonly string _accessSecretKey;
    private readonly string _refreshSecretKey;
    
    public UserCredentialRepository(ApplicationDbContext context, IMapper mapper, IConfiguration configuration,
        GenerateTokenHelper generateToken)
    {
        _context = context;
        _mapper = mapper;
        _generateToken = generateToken;
        _accessHours = configuration.GetValue<string>("HoursSettings:AccessHours")!;
        _refreshHours = configuration.GetValue<string>("HoursSettings:RefreshHours")!;
        _accessSecretKey = configuration.GetValue<string>("ApiSettings:AccessSecret")!;
        _refreshSecretKey = configuration.GetValue<string>("ApiSettings:RefreshSecret")!;
        
    }

    public async Task Register(RegistrationRequest registrationRequest, CancellationToken cancellationToken = default)
    {
        if (IsUniqueUser(registrationRequest.Email))
        {
            throw new InvalidOperationException("A user with the same email address already exists.");
        }
        
        var user = _mapper.Map<UserCredential>(registrationRequest);
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.UsersCredentials.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var user = await _context.UsersCredentials
            .FromSqlRaw("SELECT Email, Password FROM UserCredentials WHERE Email = {0} AND Password = {1}",
                loginRequest.Email, loginRequest.Password)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == default(UserCredential))
        {
            return null!;
        }

        var accessToken =
            _generateToken.GenerateToken(user.Id.ToString(), user.Role, _accessSecretKey, TimeSpan.Parse(_accessHours));
        var refreshToken =
            _generateToken.GenerateToken(user.Id.ToString(), null, _refreshSecretKey, TimeSpan.Parse(_refreshSecretKey));

        return new LoginResponse
        {
            TokenType = "Bearer",
            AccessToken = accessToken,
            ExpiresIn = (int)TimeSpan.FromHours(1).TotalSeconds,
            RefreshToken = refreshToken
        };
    }

    public async Task<ICollection<UserCredential>> Get(CancellationToken cancellationToken = default)
    {
        return await _context.UsersCredentials.FromSqlRaw("SELECT * FROM UserCredentials")
            .ToListAsync(cancellationToken);
    }

    public bool IsUniqueUser(string email)
    {
        var user = _context.UsersCredentials
            .FromSqlInterpolated($"SELECT Email FROM UserCredentials WHERE Email = {email}").FirstOrDefault();

        return user == null;
    }
}
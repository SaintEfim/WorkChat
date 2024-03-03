using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Authify.Data.Exceptions;
using Service.Authify.Data.PostgreSql.Context;
using Service.Authify.Data.Helpers;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.Data.PostgreSql.Repository
{
    public class UserCredentialRepository : IUserCredentialRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly GenerateTokenHelper _generateToken;
        private readonly string _tokenType;
        private readonly string _accessHours;
        private readonly string _refreshHours;
        private readonly string _accessSecretKey;
        private readonly string _refreshSecretKey;

        public UserCredentialRepository(ApplicationDbContext context, IConfiguration config,
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
            user.CreatedAt = DateTime.UtcNow;
            await _context.UsersCredentials.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
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

        public async Task<ICollection<UserCredential>> Get(CancellationToken cancellationToken = default)
        {
            return await _context.UsersCredentials.ToListAsync(cancellationToken);
        }

        public async Task<bool> IsUniqueUser(string email, CancellationToken cancellationToken = default)
        {
            var userExists = await _context.UsersCredentials
                .Where(u => u.Email == email)
                .SingleOrDefaultAsync(cancellationToken);

            return userExists != null;
        }

        public async Task<UserCredential?> GetUserByEmailAndPassword(LoginRequest loginRequest,
            CancellationToken cancellationToken = default)
        {
            var userExists = await _context.UsersCredentials
                .Where(u => u.Email == loginRequest.Email)
                .SingleOrDefaultAsync(cancellationToken);

            return userExists;
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
    }
}
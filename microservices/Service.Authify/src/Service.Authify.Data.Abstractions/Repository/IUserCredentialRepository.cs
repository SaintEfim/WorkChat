using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.Data.Repository;

public interface IUserCredentialRepository
{
    Task Register(
        UserCredential user,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> Login(
        UserCredential user,
        CancellationToken cancellationToken = default);

    Task<ICollection<UserCredential>> Get(
        CancellationToken cancellationToken = default);

    Task<bool> IsUniqueUser(string email, CancellationToken cancellationToken = default);

    Task<UserCredential?> GetUserByEmailAndPassword(LoginRequest loginRequest,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> Refresh(string refreshToken, CancellationToken cancellationToken = default);
}
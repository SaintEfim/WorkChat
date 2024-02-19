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

    bool IsUniqueUser(string username);

    Task<UserCredential> GetUserByEmailAndPasswordAsync(LoginRequest loginRequest,
        CancellationToken cancellationToken = default);
}
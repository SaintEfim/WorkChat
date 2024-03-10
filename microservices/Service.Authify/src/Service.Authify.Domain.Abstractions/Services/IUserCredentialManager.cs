using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.Domain.Services;

public interface IUserCredentialManager
{
    Task Register(
        RegistrationRequest registrationRequest,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> Login(
        UserCredential user,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> Refresh(
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task UpdateUser(UserCredential user, CancellationToken cancellationToken = default);
}
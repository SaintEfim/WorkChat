using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.Domain.Services;

public interface IUserCredentialManager : IDataManager<UserCredential>
{
    Task Register(
        RegistrationRequest registrationRequest,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> Login(
        LoginRequest user,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> Refresh(
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task ResetPassword(ResetPassword resetPassword, CancellationToken cancellationToken = default);
}
using AutoMapper;
using Microsoft.Extensions.Logging;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Models;

namespace Service.Authify.Domain.Services;

public class UserCredentialProvider :
    DataProviderBase<UserCredentialProvider, IUserCredentialRepository, UserCredential>,
    IUserCredentialProvider
{
    public UserCredentialProvider(IMapper mapper, ILogger<UserCredentialProvider> logger,
        IUserCredentialRepository repository) : base(mapper, logger, repository)
    {
    }
}
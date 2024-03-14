using AutoMapper;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Models;

namespace Service.Authify.Domain.Services;

public class UserCredentialProvider : DataProviderBase<IUserCredentialRepository, UserCredential>,
    IUserCredentialProvider
{
    public UserCredentialProvider(IMapper mapper, IUserCredentialRepository repository) : base(mapper, repository)
    {
    }
}
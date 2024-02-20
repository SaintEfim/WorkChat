using AutoMapper;
using Service.Authify.API.Models;
using Service.Authify.Domain.Models;

namespace Service.Authify.API;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserCredential, UserCredentialDto>();
    }
}
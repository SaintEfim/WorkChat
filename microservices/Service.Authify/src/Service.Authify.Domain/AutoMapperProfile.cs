using AutoMapper;
using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;

namespace Service.Authify.Domain;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegistrationRequest, UserCredential>()
            .ForMember(dest => dest.Role,
                opt => { opt.MapFrom(src => string.IsNullOrEmpty(src.Role) ? "user" : src.Role); });
    }
}
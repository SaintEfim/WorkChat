using AutoMapper;
using Service.Authify.API.Models;
using Service.Authify.API.Models.RequestsDto;
using Service.Authify.API.Models.ResponsesDto;
using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.API;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserCredential, UserCredentialDto>();
        CreateMap<LoginRequestDto, UserCredential>();
        CreateMap<RegistrationRequestDto, RegistrationRequest>();
        CreateMap<LoginRequestDto, LoginRequest>();
        CreateMap<LoginResponse, LoginResponseDto>();
        CreateMap<UserCredentialUpdateDto, UserCredential>().ReverseMap();
    }
}
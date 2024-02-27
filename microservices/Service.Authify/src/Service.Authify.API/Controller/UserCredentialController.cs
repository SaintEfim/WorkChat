using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.Authify.API.Models;
using Service.Authify.API.Models.RequestsDto;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Services;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Service.Authify.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserCredentialController : ControllerBase
{
    private readonly IUserCredentialManager _manager;
    private readonly IUserCredentialProvider _provider;
    private readonly IMapper _mapper;

    public UserCredentialController(IUserCredentialManager manager, IUserCredentialProvider provider, IMapper mapper)
    {
        _manager = manager;
        _provider = provider;
        _mapper = mapper;
    }

    [HttpGet]
    [SwaggerOperation(OperationId = nameof(UserCredentialGet))]
    [SwaggerResponse(Status200OK, Type = typeof(List<UserCredentialDto>))]
    public async Task<ActionResult<List<UserCredentialDto>>> UserCredentialGet(
        CancellationToken cancellationToken = default)
    {
        var users = await _provider.Get();
        return Ok(_mapper.Map<List<UserCredentialDto>>(users));
    }

    [HttpPost]
    [SwaggerOperation(OperationId = nameof(UserCredentialGet))]
    [SwaggerResponse(Status200OK)]
    public async Task<IActionResult> UserCredentialRegister(RegistrationRequestDto registrationRequest,
        CancellationToken cancellationToken = default)
    {
        await _manager.Register(_mapper.Map<RegistrationRequest>(registrationRequest));
        return Ok();
    }
}
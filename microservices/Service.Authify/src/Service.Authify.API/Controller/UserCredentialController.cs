using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.Authify.API.Models;
using Service.Authify.API.Models.RequestsDto;
using Service.Authify.API.Models.ResponsesDto;
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
        var users = await _provider.Get(cancellationToken);
        return Ok(_mapper.Map<List<UserCredentialDto>>(users));
    }

    [HttpPost("register")]
    [SwaggerOperation(OperationId = nameof(UserCredentialRegister))]
    [SwaggerResponse(Status200OK)]
    public async Task<IActionResult> UserCredentialRegister(RegistrationRequestDto registrationRequest,
        CancellationToken cancellationToken = default)
    {
        await _manager.Register(_mapper.Map<RegistrationRequest>(registrationRequest), cancellationToken);
        return Ok();
    }

    [HttpPost("login")]
    [SwaggerOperation(OperationId = nameof(UserCredentialLogin))]
    [SwaggerResponse(Status200OK)]
    public async Task<ActionResult<LoginResponseDto>> UserCredentialLogin(LoginRequestDto loginRequest,
        CancellationToken cancellationToken = default)
    {
        var res = await _manager.Login(_mapper.Map<LoginRequest>(loginRequest), cancellationToken);
        return Ok(_mapper.Map<LoginResponseDto>(res));
    }

    [HttpPost("refresh")]
    [SwaggerOperation(OperationId = nameof(UserCredentialRefresh))]
    [SwaggerResponse(Status200OK)]
    public async Task<ActionResult<LoginResponseDto>> UserCredentialRefresh(string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var res = await _manager.Refresh(refreshToken, cancellationToken);
        return Ok(_mapper.Map<LoginResponseDto>(res));
    }

    [HttpPatch("resetPassword")]
    [SwaggerOperation(OperationId = nameof(UserCredentialResetPassword))]
    [SwaggerResponse(Status200OK)]
    [SwaggerResponse(Status404NotFound)]
    public async Task<IActionResult> UserCredentialResetPassword(ResetPasswordDto resetPassword,
        CancellationToken cancellationToken = default)
    {
        await _manager.ResetPassword(_mapper.Map<ResetPassword>(resetPassword), cancellationToken);
        return Ok();
    }
}
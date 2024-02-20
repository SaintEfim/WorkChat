using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.Authify.API.Models;
using Service.Authify.Domain.Services;

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
    public async Task<ActionResult<List<UserCredentialDto>>> Get(CancellationToken cancellationToken = default)
    {
        var users = await _provider.Get();
        return Ok(_mapper.Map<List<UserCredentialDto>>(users));
    }
}
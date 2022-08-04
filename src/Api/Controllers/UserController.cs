using Api.Models.Request;
using Api.Models.Response;
using Application.IServices;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUserService _service;

    public UserController(IUserService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    //TODO: Swagger doc
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);

        var result = await _service.CreateAsync(user);

        if (result is null)
            return BadRequest();

        var userResponse = _mapper.Map<UserResponse>(result);

        return Created(nameof(CreateAsync), userResponse);
    }

    //TODO: Swagger doc
    [HttpPatch("update-email/{id}")]
    public async Task<IActionResult> UpdateEmailAsync([FromRoute] Guid id, [FromBody] UpdateEmailRequest request)
    {
        if (await _service.UpdateEmailAsync(id, request.Email))
            return NoContent();

        return BadRequest();
    }

    //TODO: Swagger doc
    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] Guid id)
    {
        var result = _service.Get(id);

        if (result is null)
            return NotFound();

        var userResponse = _mapper.Map<UserResponse>(result);
        return Ok(userResponse);
    }
}
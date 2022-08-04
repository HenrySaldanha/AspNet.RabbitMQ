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


    /// <summary>
    /// Create a new user (Email and phone must be valid)
    /// </summary>
    /// <param name="request"> User attributes </param>
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Update user email (Email must be valid and unique)
    /// </summary>
    /// <param name="request"> Email value </param>
    /// <param name="id"> User Id </param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPatch("update-email/{id}")]
    public async Task<IActionResult> UpdateEmailAsync([FromRoute] Guid id, [FromBody] UpdateEmailRequest request)
    {
        if (await _service.UpdateEmailAsync(id, request.Email))
            return NoContent();

        return BadRequest();
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="id"> User Id </param>
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
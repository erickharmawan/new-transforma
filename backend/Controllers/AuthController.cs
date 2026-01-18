using Microsoft.AspNetCore.Mvc;
using backend.Contracts;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        
        if (result == null)
            return Unauthorized(new { message = "Invalid email or password" });

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto createUserDto)
    {
        var result = await _authService.RegisterAsync(createUserDto);
        
        if (result == null)
            return BadRequest(new { message = "Email already exists" });

        return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
    }
}

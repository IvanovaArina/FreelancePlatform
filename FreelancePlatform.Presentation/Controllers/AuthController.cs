using Microsoft.AspNetCore.Mvc;
using FreelancePlatform.Application.Services;
using FreelancePlatform.Application.DTOs;

namespace FreelancePlatform.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register/{role}")]
    public async Task<IActionResult> Register(string role, [FromBody] RegisterRequest request)
    {
        var user = await _authService.RegisterAsync(role, request.Email, request.Password, request.Name);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(request.Email, request.Password);
        return Ok(new { Token = token });
    }
}

public record RegisterRequest(string Email, string Password, string Name);
public record LoginRequest(string Email, string Password);
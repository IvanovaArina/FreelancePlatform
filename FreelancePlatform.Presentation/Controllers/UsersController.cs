using Microsoft.AspNetCore.Mvc;
using FreelancePlatform.Application.Services;
using FreelancePlatform.Application.DTOs;

namespace FreelancePlatform.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create/{role}")]
    public async Task<IActionResult> CreateUser(string role, [FromBody] CreateUserRequest request)
    {
        var user = await _userService.CreateUserAsync(role, request.Email, request.Name);
        return Ok(user);
    }
}

public record CreateUserRequest(string Email, string Name);
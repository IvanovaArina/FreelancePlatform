using AuthService.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using AuthService.Services;

namespace AuthService.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthApplicationService _auth;

    public AuthController(AuthApplicationService auth) => _auth = auth;

    [HttpPost("signup")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
    {
        try { return Ok(await _auth.Register(req)); }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("signin")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
    {
        try { return Ok(await _auth.Login(req)); }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }
}
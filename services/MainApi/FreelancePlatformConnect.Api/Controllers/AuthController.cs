using FreelancePlatformConnect.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly HttpClient _http;

    public AuthController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var response = await _http.PostAsJsonAsync(
            "http://auth-service:8080/api/auth/signup",
            req
        );

        var body = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, body);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var response = await _http.PostAsJsonAsync(
            "http://auth-service:8080/api/auth/signin",
            req
        );

        var body = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, body);
    }
}

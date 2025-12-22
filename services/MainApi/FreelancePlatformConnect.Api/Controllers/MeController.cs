using FreelancePlatformConnect.Api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

[Route("api/me")]
[ApiController]
[Authorize]
public class MeController : ControllerBase
{
    private readonly HttpClient _http;

    public MeController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
    }

    [HttpGet("name")]
    public async Task<IActionResult> GetName()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "http://auth-service:8080/api/me/name"
        );

        // ✅ корректное пробрасывание JWT
        request.Headers.Add(
            "Authorization",
            Request.Headers["Authorization"].ToString()
        );

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        return StatusCode((int)response.StatusCode, body);
    }

    [HttpPut("name")]
    public async Task<IActionResult> ChangeName([FromBody] ChangeNameRequest req)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Put,
            "http://auth-service:8080/api/me/name"
        )
        {
            Content = JsonContent.Create(req)
        };

        // ✅ корректное пробрасывание JWT
        request.Headers.Add(
            "Authorization",
            Request.Headers["Authorization"].ToString()
        );

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        return StatusCode((int)response.StatusCode, body);
    }
}

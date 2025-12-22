using FreelancePlatformConnect.Api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

[Route("api/freelancer/profile")]
[ApiController]
[Authorize(Roles = "freelancer")]
public class FreelancerProfileController : ControllerBase
{
    private readonly HttpClient _http;

    public FreelancerProfileController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "http://profile-service:8080/api/freelancer/profile/me"
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

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest req)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Put,
            "http://profile-service:8080/api/freelancer/profile/me"
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

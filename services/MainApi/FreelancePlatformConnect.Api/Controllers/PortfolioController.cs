using FreelancePlatformConnect.Api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FreelancePlatformConnect.Api.Controllers;

[Route("api/portfolio")]
[ApiController]
[Authorize(Roles = "freelancer")]
public class PortfolioController : ControllerBase
{
    private readonly HttpClient _http;

    public PortfolioController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
    }

    private void ForwardAuth(HttpRequestMessage request)
    {
        if (Request.Headers.TryGetValue("Authorization", out var auth))
        {
            request.Headers.Authorization =
                AuthenticationHeaderValue.Parse(auth.ToString());
        }
    }

    // GET api/portfolio
    [HttpGet]
    public async Task<IActionResult> GetMyPortfolio()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "http://portfolio-service:8080/api/portfolio"
        );

        ForwardAuth(request);

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        return StatusCode((int)response.StatusCode, body);
    }

    // POST api/portfolio
    [HttpPost]
    public async Task<IActionResult> Create(CreatePortfolioItemRequest req)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "http://portfolio-service:8080/api/portfolio"
        )
        {
            Content = JsonContent.Create(req)
        };

        ForwardAuth(request);

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        return StatusCode((int)response.StatusCode, body);
    }

    // GET api/portfolio/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"http://portfolio-service:8080/api/portfolio/{id}"
        );

        ForwardAuth(request);

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        return StatusCode((int)response.StatusCode, body);
    }

    // PUT api/portfolio/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePortfolioItemRequest req)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Put,
            $"http://portfolio-service:8080/api/portfolio/{id}"
        )
        {
            Content = JsonContent.Create(req)
        };

        ForwardAuth(request);

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        return StatusCode((int)response.StatusCode, body);
    }

    // DELETE api/portfolio/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Delete,
            $"http://portfolio-service:8080/api/portfolio/{id}"
        );

        ForwardAuth(request);

        var response = await _http.SendAsync(request);

        return StatusCode((int)response.StatusCode);
    }
}

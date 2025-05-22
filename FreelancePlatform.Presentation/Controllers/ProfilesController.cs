using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FreelancePlatform.Application.Services;
using FreelancePlatform.Application.DTOs;
using System.Security.Claims;

namespace FreelancePlatform.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Freelancer")]
public class ProfilesController : ControllerBase
{
    private readonly ProfileService _profileService;

    public ProfilesController(ProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequest request)
    {
        var profile = await _profileService.CreateProfileAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, request.Name, request.Skill, request.PortfolioItem, request.Review);
        return Ok(profile);
    }
}

public record CreateProfileRequest(string Name, string Skill, string PortfolioItem, string Review);
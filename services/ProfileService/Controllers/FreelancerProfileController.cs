using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Models.DTOs;
using ProfileService.Repositories.Interfaces;
using System.Security.Claims;

namespace ProfileService.Controllers;

[Route("api/freelancer/profile")]
[ApiController]
public class FreelancerProfileController : ControllerBase
{
    private readonly IFreelancerProfileRepository _profiles;
    private readonly IMapper _mapper;

    public FreelancerProfileController(IFreelancerProfileRepository profiles, IMapper mapper)
    {
        _profiles = profiles;
        _mapper = mapper;
    }

    private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpGet("me")]
    [Authorize(Roles = "freelancer")]
    public async Task<ActionResult<FreelancerProfileDto>> GetMyProfile()
    {
        var profile = await _profiles.GetByUserIdAsync(UserId);
        if (profile == null) return NotFound("Профиль не найден");

        return Ok(_mapper.Map<FreelancerProfileDto>(profile));
    }

    [HttpPut("me")]
    [Authorize(Roles = "freelancer")]
    public async Task<IActionResult> UpdateMyProfile(UpdateProfileRequest request)
    {
        var profile = await _profiles.GetByUserIdAsync(UserId);
        if (profile == null) return NotFound("Профиль не найден");

        profile.JobTitle = request.JobTitle;
        profile.Bio = request.Bio;
        profile.Skills = request.Skills;
        profile.HourlyRate = request.HourlyRate;
        profile.ProfileImageUrl = request.ProfileImageUrl;

        await _profiles.UpdateAsync(profile);

        return Ok(_mapper.Map<FreelancerProfileDto>(profile));
    }
}

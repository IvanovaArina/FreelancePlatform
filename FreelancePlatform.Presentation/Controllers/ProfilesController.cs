using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FreelancePlatform.Application.Services;
using FreelancePlatform.Application.DTOs;
using System.Security.Claims;
using FreelancePlatform.Application.DesignPatterns.Observer; // Для NotificationService, если будем добавлять

namespace FreelancePlatform.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Freelancer")]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfileService _profileService;
        private readonly NotificationService _notificationService; // Для уведомлений

        public ProfilesController(ProfileService profileService, NotificationService notificationService)
        {
            _profileService = profileService;
            _notificationService = notificationService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name) || request.Skills == null || !request.Skills.Any())
                return BadRequest("Name and at least one Skill are required.");

            var freelancerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(freelancerId))
                return Unauthorized("Freelancer ID not found in token.");

            try
            {
                var profile = await _profileService.CreateProfileAsync(freelancerId, request.Name, request.Skills, request.Portfolio, request.Reviews);
                _notificationService.Notify($"Profile created for {request.Name} with ID: {profile.Id}");
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileByUserId(string userId)
        {
            var freelancerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(freelancerId))
                return Unauthorized("Freelancer ID not found in token.");

            try
            {
                var profile = await _profileService.GetProfileByUserIdAsync(userId);
                if (profile.UserId != freelancerId)
                    return Forbid("You can only access your own profile.");
                _notificationService.Notify($"Profile retrieved for {profile.Name} with ID: {profile.Id}"); // Уведомление
                return Ok(profile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public record CreateProfileRequest(string Name, List<string> Skills, List<string> Portfolio, List<string> Reviews);
}
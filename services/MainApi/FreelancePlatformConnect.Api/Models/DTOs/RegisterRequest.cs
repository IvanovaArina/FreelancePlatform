namespace FreelancePlatformConnect.Api.Models.DTOs;
public record RegisterRequest(
    string Name,
    string Email,
    string Password,
    string ConfirmPassword,
    string Role = "freelancer");


namespace FreelancePlatformConnect.Api.Models.DTOs;

public record UpdateProfileRequest(
    string JobTitle,
    string Bio,
    List<string> Skills,
    decimal HourlyRate,
    string? ProfileImageUrl = null
);
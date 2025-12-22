namespace ProfileService.Models.DTOs;

public record FreelancerProfileDto(
    string JobTitle,
    string Bio,
    List<string> Skills,
    decimal HourlyRate,
    bool Verified,
    string? ProfileImageUrl
);

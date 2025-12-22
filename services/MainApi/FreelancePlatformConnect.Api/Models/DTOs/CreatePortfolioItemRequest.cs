namespace FreelancePlatformConnect.Api.Models.DTOs;
public record CreatePortfolioItemRequest(
    string Title,
    string Description,
    string? TempImageKey);
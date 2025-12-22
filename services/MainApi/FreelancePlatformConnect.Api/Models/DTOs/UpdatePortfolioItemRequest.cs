namespace FreelancePlatformConnect.Api.Models.DTOs;
public record UpdatePortfolioItemRequest(
    string Title, 
    string Description,
    string? TempImageKey);
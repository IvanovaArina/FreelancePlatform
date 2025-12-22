namespace FreelancePlatformConnect.Api.Models.DTOs;
public record PortfolioItemDto(
    int Id, 
    string Title, 
    string Description, 
    string ImageUrl, 
    DateTime CreatedAt);
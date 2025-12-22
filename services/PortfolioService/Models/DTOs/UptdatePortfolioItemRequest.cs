namespace PortfolioService.Models.DTOs;
public record UpdatePortfolioItemRequest(
    string Title,
    string Description,
    string? TempImageKey);
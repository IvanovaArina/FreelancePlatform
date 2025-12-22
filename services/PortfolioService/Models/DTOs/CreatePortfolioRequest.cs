namespace PortfolioService.Models.DTOs;
public record CreatePortfolioItemRequest(
    string Title,
    string Description,
    string? TempImageKey);

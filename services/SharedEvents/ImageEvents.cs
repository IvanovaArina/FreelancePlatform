// services/SharedEvents/ImageEvents.cs
namespace SharedEvents;

public record ProjectImageUploaded(int PortfolioId, string TempImageKey);
public record ImageProcessed(int PortfolioId, string FinalImageUrl);
public record ProjectDeleted(int PortfolioId, string? ImageUrl);

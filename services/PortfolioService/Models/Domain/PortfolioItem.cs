namespace PortfolioService.Models.Domain
{
    public class PortfolioItem
    {
        public int Id { get; set; }
        public int FreelancerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string? TempImageKey { get; set; }      // ← новое
        public string ImageStatus { get; set; } = "none"; // ← новое
        public string? ImageUrl { get; set; }          // ← теперь nullable

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

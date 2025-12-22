namespace FreelancePlatformConnect.Api.Models.Domain
{
    public class FreelancerProfile
    {
        public int UserId { get; set; }                    // Это и PK, и FK одновременно!
        public string JobTitle { get; set; } = "Freelancer";
        public string Bio { get; set; } = "";
        public List<string> Skills { get; set; } = new();
        public decimal HourlyRate { get; set; } = 0;
        public string? ProfileImageUrl { get; set; }
        public bool Verified { get; set; } = false;

        public User User { get; set; } = null!;
    }
}

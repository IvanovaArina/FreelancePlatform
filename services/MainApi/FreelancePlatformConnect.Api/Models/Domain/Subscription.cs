namespace FreelancePlatformConnect.Api.Models.Domain
{
    public class Subscription
    {
        public int Id { get; set; }
        public int FreelancerId { get; set; }  

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty; // "$299/month"
        public bool IsActive { get; set; } = true;

        public List<string> WhatIncludes { get; set; } = new(); // → jsonb

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

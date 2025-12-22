namespace FreelancePlatformConnect.Api.Models.DTOs;
public record LoginRequest(
    string Email, 
    string Password, 
    bool RememberMe = false);


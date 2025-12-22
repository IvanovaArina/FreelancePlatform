namespace AuthService.Models.DTOs;
public record LoginRequest(
    string Email,
    string Password,
    bool RememberMe = false);


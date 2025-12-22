namespace AuthService.Models.DTOs;

public record AuthResponse(
    string Token,
    DateTime ExpiresAt,
    UserDto User
);
namespace AuthService.Models.DTOs;

public record UserDto(
    int Id,
    string Name,
    string Email,
    string Role
);

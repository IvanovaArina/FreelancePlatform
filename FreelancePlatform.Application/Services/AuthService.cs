using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Infrastructure.Repository;
using FreelancePlatform.Application.DTOs;
using FreelancePlatform.Application.Mappers;

namespace FreelancePlatform.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserCredentialsRepository _credentialsRepository;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepository, IUserCredentialsRepository credentialsRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _credentialsRepository = credentialsRepository;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<UserDto> RegisterAsync(string role, string email, string password, string name)
    {
        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null) throw new InvalidOperationException("User already exists");

        var userService = new UserService(_userRepository, new AutoMapper.Mapper(new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>())));
        var user = await userService.CreateUserAsync(role, email, name);

        var credentials = new UserCredentials
        {
            UserId = user.Id,
            PasswordHash = _passwordHasher.HashPassword(new User(), password)
        };
        await _credentialsRepository.AddAsync(credentials);

        return new UserDto { Id = user.Id, Name = user.Name, Email = user.Email, Role = user.Role };
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) throw new InvalidOperationException("Invalid email or password");

        var credentials = await _credentialsRepository.GetByUserIdAsync(user.Id);
        if (credentials == null) throw new InvalidOperationException("Invalid email or password");

        var result = _passwordHasher.VerifyHashedPassword(new User(), credentials.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed) throw new InvalidOperationException("Invalid email or password");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
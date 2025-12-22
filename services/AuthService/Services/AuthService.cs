using AuthService.Models.Domain;
using AuthService.Models.DTOs;
using AuthService.Repositories.Interfaces;
using AutoMapper;
using MassTransit;
using Microsoft.IdentityModel.Tokens;
using SharedEvents;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class AuthApplicationService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IPublishEndpoint _publish;

        public AuthApplicationService(
            IUserRepository userRepo,
            IMapper mapper,
            IConfiguration config,
            IPublishEndpoint publish)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
            _publish = publish;
        }

        public async Task<AuthResponse> Register(RegisterRequest req)
        {
            if (req.Password != req.ConfirmPassword)
                throw new Exception("Пароли не совпадают");

            if (await _userRepo.GetByEmailAsync(req.Email) != null)
                throw new Exception("Email уже занят");

            var user = new User
            {
                Name = req.Name,
                Email = req.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = req.Role.ToLower()
            };

            await _userRepo.AddAsync(user);

            // 🔔 СООБЩАЕМ ДРУГИМ СЕРВИСАМ
            await _publish.Publish(new UserRegistered(user.Id, user.Role));

            var expires = DateTime.UtcNow.AddHours(1);
            var token = GenerateToken(user, expires);

            return new AuthResponse(
                token,
                expires,
                _mapper.Map<UserDto>(user)
            );
        }

        public async Task<AuthResponse> Login(LoginRequest req)
        {
            var user = await _userRepo.GetByEmailAsync(req.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                throw new Exception("Неверный логин или пароль");

            var expires = req.RememberMe
                ? DateTime.UtcNow.AddDays(30)
                : DateTime.UtcNow.AddHours(1);

            var token = GenerateToken(user, expires);

            return new AuthResponse(
                token,
                expires,
                _mapper.Map<UserDto>(user)
            );
        }

        private string GenerateToken(User user, DateTime expires)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

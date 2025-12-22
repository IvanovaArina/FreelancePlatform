using AuthService.Models.Domain;
using AuthService.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AuthService.Models.DTOs;

namespace AuthService.Controllers
{
    
    [Route("api/me")]
    [ApiController]
    [Authorize] // любой авторизованный пользователь (и фрилансер, и работодатель)
    public class MeController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public MeController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Получить своё имя (и другую инфу о себе)
        [HttpGet("name")]
        public async Task<ActionResult<string>> GetName()
        {
            var user = await _userRepo.GetByIdAsync(UserId);
            if (user == null) return NotFound();
            return Ok(user.Name);
        }

        // Изменить своё имя
        [HttpPut("name")]
        public async Task<IActionResult> ChangeName(ChangeNameRequest request)
        {
            var user = await _userRepo.GetByIdAsync(UserId);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(request.NewName))
                return BadRequest("Имя не может быть пустым");

            user.Name = request.NewName.Trim();
            await _userRepo.UpdateAsync(user); // ← добавь этот метод в репозиторий!

            return NoContent();
        }

        // Бонус: получить всю инфу о себе (имя + роль + email)
        //[HttpGet]
        //public async Task<ActionResult<UserDto>> GetMe()
        //{
        //    var user = await _userRepo.GetByIdAsync(UserId);
        //    if (user == null) return NotFound();
        //    return Ok(_mapper.Map<UserDto>(user));
        //}
    }
}


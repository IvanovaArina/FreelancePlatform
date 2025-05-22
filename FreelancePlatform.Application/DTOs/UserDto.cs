using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Этот класс — DTO для сущности User. Используется для передачи данных о пользователях в API,
// включая поле Name. Почему такой набор? Он отражает данные, которые нужно передать наружу,
// исключая лишние детали.

namespace FreelancePlatform.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}

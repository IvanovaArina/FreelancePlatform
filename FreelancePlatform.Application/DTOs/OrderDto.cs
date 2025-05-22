using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Этот класс — DTO (Data Transfer Object) для сущности Order.
// Он используется для передачи данных о заказах в API, избегая прямой передачи самой сущности,
// что улучшает безопасность и инкапсуляцию. Почему такой набор полей? Он соответствует сущности Order,
// но используется для внешнего API.

namespace FreelancePlatform.Application.DTOs
{
    public class OrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int Hours { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string FreelancerId { get; set; } = string.Empty;
        public bool IsUrgent { get; set; }
        public bool HasPremiumSupport { get; set; }
    }
}

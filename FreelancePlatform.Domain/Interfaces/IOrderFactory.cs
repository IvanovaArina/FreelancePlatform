using FreelancePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Domain.Interfaces
{

    // Этот интерфейс используется для реализации паттерна Factory Method.
    // Он определяет метод для создания заказов разных типов (например, "Design", "Coding", "Marketing").
    // Позволяет создавать заказы, не привязываясь к конкретной реализации.

    public interface IOrderFactory
    {
        Order CreateOrder(string title, decimal basePrice);
    }
}

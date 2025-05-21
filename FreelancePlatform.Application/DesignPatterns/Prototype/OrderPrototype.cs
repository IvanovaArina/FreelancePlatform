using FreelancePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Этот класс реализует паттерн Prototype (порождающий паттерн).
// Он содержит статический метод для клонирования объекта Order,
// создавая копию заказа с новым идентификатором. Почему Prototype?
// Это позволяет быстро создавать дубликаты заказов без повторного ввода данных.

namespace FreelancePlatform.Application.DesignPatterns.Prototype
{
    public static class OrderPrototype
    {
        public static Order CloneOrder(Order original)
        {
            return new Order
            {
                Id = Guid.NewGuid().ToString(),
                Title = original.Title,
                Category = original.Category,
                BasePrice = original.BasePrice,
                Hours = original.Hours,
                ClientId = original.ClientId
            };
        }
    }
}

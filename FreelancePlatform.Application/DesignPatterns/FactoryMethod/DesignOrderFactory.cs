// Эти классы реализуют паттерн Factory Method (порождающий паттерн) через интерфейс IOrderFactory.
// Они создают заказы разных категорий (дизайн, программирование, маркетинг), позволяя легко добавлять
// новые типы заказов. Почему именно такие реализации? Они просты и фокусируются на создании заказов с
// базовыми параметрами, оставляя расширенную логику сервисам.
using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Domain.Interfaces;

namespace FreelancePlatform.Application.DesignPatterns.FactoryMethod
{
    public class DesignOrderFactory : IOrderFactory
    {
        public Order CreateOrder(string title, decimal basePrice)
        {
            return new Order { Title = title, Category = "Design", BasePrice = basePrice };
        }
        public string GetCategory() => "Design";
    }
}

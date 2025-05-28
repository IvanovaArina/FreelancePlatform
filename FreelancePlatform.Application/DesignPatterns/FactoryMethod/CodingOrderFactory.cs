using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Domain.Interfaces;


namespace FreelancePlatform.Application.DesignPatterns.FactoryMethod
{
    public class CodingOrderFactory : IOrderFactory
    {
        public Order CreateOrder(string title, decimal basePrice)
        {
            return new Order { Title = title, Category = "Coding", BasePrice = basePrice };
        }
        public string GetCategory() => "Coding";
    }

}

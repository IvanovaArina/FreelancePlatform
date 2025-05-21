

using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Domain.Interfaces;

namespace FreelancePlatform.Application.DesignPatterns.FactoryMethod
{
    internal class MarketingOrderFactory : IOrderFactory
    {
        public Order CreateOrder(string title, decimal basePrice)
        {
            return new Order { Title = title, Category = "Marketing", BasePrice = basePrice };
        }
    }
}

using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Domain.Interfaces;


namespace FreelancePlatform.Application.DesignPatterns.Decorator
{
    public class BasicOrder : IOrderComponent
    {
        private readonly Order _order;
        public BasicOrder(Order order) => _order = order;
        public string Description => _order.Title;
        public decimal Cost => _order.BasePrice;
    }
}

using FreelancePlatform.Application.DesignPatterns.Observer;
using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Domain.Interfaces;
using FreelancePlatform.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Command
{
    public class CreateOrderCommand : ICommand
    {
        private readonly IOrderFactory _factory;
        private readonly NotificationService _notificationService;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly string _title;
        private readonly decimal _basePrice;
        private readonly string _clientId;
        public Order CreatedOrder { get; private set; }

        public CreateOrderCommand(IOrderFactory factory, NotificationService notificationService, IOrderRepository orderRepository, IUserRepository userRepository, string title, decimal basePrice, string clientId)
        {
            _factory = factory;
            _notificationService = notificationService;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _title = title;
            _basePrice = basePrice;
            _clientId = clientId;
        }
        public void Execute()
        {
            var client = _userRepository.GetByIdAsync(_clientId).GetAwaiter().GetResult();
            if (client == null || client.Role != "Client")
                throw new InvalidOperationException("Only clients can create orders");

            CreatedOrder = _factory.CreateOrder(_title, _basePrice);
            CreatedOrder.ClientId = _clientId;
            _orderRepository.AddAsync(CreatedOrder).GetAwaiter().GetResult();
            _notificationService.Notify($"New order created: {_title}");
        }
    }
}

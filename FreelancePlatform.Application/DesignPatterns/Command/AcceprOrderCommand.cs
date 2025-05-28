using FreelancePlatform.Application.DesignPatterns.Observer;
using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Domain.Interfaces;
using FreelancePlatform.Infrastructure.Repository;
using System;

namespace FreelancePlatform.Application.DesignPatterns.Command
{
    public class AcceptOrderCommand : ICommand
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly NotificationService _notificationService;
        private readonly string _orderId;
        private readonly string _freelancerId;
        public Order AcceptedOrder { get; private set; }

        public AcceptOrderCommand(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            NotificationService notificationService,
            string orderId,
            string freelancerId)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _orderId = orderId;
            _freelancerId = freelancerId;
        }

        public void Execute()
        {
            var order = _orderRepository.GetByIdAsync(_orderId).GetAwaiter().GetResult();
            if (order == null)
                throw new InvalidOperationException("Order not found");

            if (!string.IsNullOrEmpty(order.FreelancerId))
                throw new InvalidOperationException("Order already has a freelancer assigned");

            var freelancer = _userRepository.GetByIdAsync(_freelancerId).GetAwaiter().GetResult();
            if (freelancer == null || freelancer.Role != "Freelancer")
                throw new InvalidOperationException("Invalid freelancer");

            order.FreelancerId = _freelancerId;
            _orderRepository.UpdateAsync(order).GetAwaiter().GetResult();
            AcceptedOrder = order;

            _notificationService.Notify($"Order {order.Title} accepted by freelancer {_freelancerId}");
        }
    }
}
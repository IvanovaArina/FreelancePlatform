using AutoMapper;
using FreelancePlatform.Application.DesignPatterns.Command;
using FreelancePlatform.Application.DesignPatterns.Observer;
using FreelancePlatform.Application.DesignPatterns.Prototype;
using FreelancePlatform.Application.DTOs;
using FreelancePlatform.Domain.Interfaces;
using FreelancePlatform.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.Services
{
    // Этот класс содержит бизнес-логику для управления заказами: создание, клонирование, улучшение (добавление срочности/премиум-поддержки),
    // получение активных заказов. Использует паттерны Factory Method, Prototype, Observer. Теперь создание заказа использует паттерн Command через CreateOrderCommand.
    // Убрали использование Singleton (OrderManager), заменив его на вызов репозитория для получения активных заказов. Почему такие методы? Они покрывают основные сценарии работы с заказами,
    // а остальная логика (например, удаление) не требуется по бизнес-требованиям.
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly CommandInvoker _commandInvoker;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IMapper mapper, CommandInvoker commandInvoker)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _commandInvoker = commandInvoker;
        }

        public async Task<OrderDto> CreateOrderAsync(string title, decimal basePrice, string clientId, IOrderFactory factory, NotificationService notificationService)
        {
            var command = new CreateOrderCommand(factory, notificationService, _orderRepository, _userRepository, title, basePrice, clientId);
            _commandInvoker.ExecuteCommand(command);
            return _mapper.Map<OrderDto>(command.CreatedOrder);
        }

        public async Task<OrderDto> CloneOrderAsync(string orderId, NotificationService notificationService)
        {
            var original = await _orderRepository.GetByIdAsync(orderId);
            if (original == null) throw new InvalidOperationException("Order not found");
            var cloned = OrderPrototype.CloneOrder(original);
            await _orderRepository.AddAsync(cloned);
            notificationService.Notify($"Order cloned: {cloned.Title}");
            return _mapper.Map<OrderDto>(cloned);
        }

        public async Task<OrderDto> EnhanceOrderAsync(string orderId, bool isUrgent, bool hasPremiumSupport)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new InvalidOperationException("Order not found");

            order.IsUrgent = isUrgent;
            order.HasPremiumSupport = hasPremiumSupport;
            await _orderRepository.UpdateAsync(order);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IReadOnlyList<OrderDto>> GetActiveOrdersAsync()
        {
            var orders = await _orderRepository.GetActiveOrdersAsync();
            return _mapper.Map<IReadOnlyList<OrderDto>>(orders);
        }

        public IOrderRepository GetOrderRepository() => _orderRepository;
        public IUserRepository GetUserRepository() => _userRepository;
        public IMapper GetMapper() => _mapper;

    }
}

using FreelancePlatform.Application.DTOs;
using FreelancePlatform.Application.Services;
using FreelancePlatform.Application.DesignPatterns.FactoryMethod;
using FreelancePlatform.Application.DesignPatterns.Observer;
using FreelancePlatform.Domain.Interfaces;
using FreelancePlatform.Application.DesignPatterns.Strategy;

// Этот класс реализует паттерн Facade (структурный паттерн).
// Он предоставляет упрощённый интерфейс для работы с заказами, объединяя вызовы OrderService, PaymentService и NotificationService.
// Например, метод CreateAndPayOrderAsync создаёт заказ и сразу оплачивает его, скрывая сложность взаимодействия между сервисами.
// Почему Facade? Это упрощает использование API, так как клиент может выполнить сложную операцию одним вызовом.
namespace FreelancePlatform.Application.DesignPatterns.Facade
{
    public class OrderFacade
    {
        private readonly OrderService _orderService;
        private readonly PaymentService _paymentService;
        private readonly NotificationService _notificationService;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly OrderPaymentContext _paymentContext;
        private readonly Dictionary<string, IOrderFactory> _factories;

        public OrderFacade(
            OrderService orderService,
            PaymentService paymentService,
            NotificationService notificationService,
            IPaymentProcessor paymentProcessor,
            OrderPaymentContext paymentContext)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _notificationService = notificationService;
            _paymentProcessor = paymentProcessor;
            _paymentContext = paymentContext;
            _factories = new Dictionary<string, IOrderFactory>
            {
                { "design", new DesignOrderFactory() },
                { "coding", new CodingOrderFactory() },
                { "marketing", new MarketingOrderFactory() }
            };
        }

        public async Task<(OrderDto Order, TransactionDto Transaction)> CreateAndPayOrderAsync(string orderType, string title, decimal basePrice, string clientId, int hours, string paymentMethod)
        {
            if (!_factories.TryGetValue(orderType.ToLower(), out var factory))
                throw new ArgumentException("Invalid order type");

            var order = await _orderService.CreateOrderAsync(title, basePrice, clientId, factory, _notificationService);
            var transaction = await _paymentService.ProcessPaymentAsync(order.Id, basePrice, hours, paymentMethod, _paymentProcessor, _paymentContext);
            return (order, transaction);
        }
    }
}
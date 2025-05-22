using FreelancePlatform.Application.DesignPatterns.Strategy;
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
    public class PayOrderCommand : ICommand
    {
        private readonly IPaymentProcessor _processor;
        private readonly OrderPaymentContext _paymentContext;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly string _orderId;
        private readonly decimal _baseAmount;
        private readonly int _hours;
        private readonly string _paymentMethod;
        public Transaction CreatedTransaction { get; private set; }

        public PayOrderCommand(IPaymentProcessor processor, OrderPaymentContext paymentContext, ITransactionRepository transactionRepository, IOrderRepository orderRepository, string orderId, decimal baseAmount, int hours, string paymentMethod)
        {
            _processor = processor;
            _paymentContext = paymentContext;
            _transactionRepository = transactionRepository;
            _orderRepository = orderRepository;
            _orderId = orderId;
            _baseAmount = baseAmount;
            _hours = hours;
            _paymentMethod = paymentMethod;
        }

        public void Execute()
        {
            var order = _orderRepository.GetByIdAsync(_orderId).GetAwaiter().GetResult();
            if (order == null) throw new InvalidOperationException("Order not found");

            var cost = _paymentContext.CalculateCost(_baseAmount, _hours);
            var success = _processor.ProcessPayment(cost);
            if (success)
            {
                CreatedTransaction = new Transaction { OrderId = _orderId, Amount = cost, PaymentMethod = _paymentMethod };
                _transactionRepository.AddAsync(CreatedTransaction).GetAwaiter().GetResult();
            }
            else
            {
                throw new InvalidOperationException("Payment failed");
            }
        }
    }
}

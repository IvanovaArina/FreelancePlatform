using AutoMapper;
using FreelancePlatform.Application.DesignPatterns.Command;
using FreelancePlatform.Application.DesignPatterns.Strategy;
using FreelancePlatform.Application.DTOs;
using FreelancePlatform.Domain.Interfaces;
using FreelancePlatform.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Этот класс содержит бизнес-логику для обработки платежей. Использует паттерны Adapter (структурный) и Strategy (поведенческий).
// Теперь оплата заказа использует паттерн Command через PayOrderCommand. Создаёт транзакцию, если платёж успешен.
// Почему только обработка? Другие операции (например, возврат) не предусмотрены.

namespace FreelancePlatform.Application.Services
{
    public class PaymentService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly CommandInvoker _commandInvoker;

        public PaymentService(ITransactionRepository transactionRepository, IOrderRepository orderRepository, IMapper mapper, CommandInvoker commandInvoker)
        {
            _transactionRepository = transactionRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _commandInvoker = commandInvoker;
        }

        public async Task<TransactionDto> ProcessPaymentAsync(string orderId, decimal baseAmount, int hours, string paymentMethod, IPaymentProcessor processor, OrderPaymentContext paymentContext)
        {
            var command = new PayOrderCommand(processor, paymentContext, _transactionRepository, _orderRepository, orderId, baseAmount, hours, paymentMethod);
            _commandInvoker.ExecuteCommand(command);
            return _mapper.Map<TransactionDto>(command.CreatedTransaction);
        }

        public ITransactionRepository GetTransactionRepository() => _transactionRepository;
        public IOrderRepository GetOrderRepository() => _orderRepository;
        public IMapper GetMapper() => _mapper;
    }
}

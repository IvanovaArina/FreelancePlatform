using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Domain.Interfaces
{
    // Этот интерфейс используется для паттерна Adapter.
    // Он определяет метод для обработки платежей, позволяя интегрировать разные платёжные системы
    // (например, Stripe, PayPal) через единый интерфейс.
    public interface IPaymentProcessor
    {
        bool ProcessPayment(decimal amount);    
    }
}

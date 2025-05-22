using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Adapter
{
    public class PayPalAdapter : IPaymentProcessor
    {
        private readonly PayPalPaymentService _payPal = new PayPalPaymentService();
        public bool ProcessPayment(decimal amount) => _payPal.SendPayment(amount);
    }
}

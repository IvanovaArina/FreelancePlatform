using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Adapter
{
    public class StripeAdapter : IPaymentProcessor
    {
        private readonly StripePaymentService _stripe = new StripePaymentService();
        public bool ProcessPayment(decimal amount) => _stripe.Charge(amount);
    }
}

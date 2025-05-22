using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Strategy
{
    public class OrderPaymentContext
    {
        private IPaymentStrategy _strategy;
        public void SetStrategy(IPaymentStrategy strategy) => _strategy = strategy;
        public decimal CalculateCost(decimal baseAmount, int hours) => _strategy.CalculateCost(baseAmount, hours);
    }
}

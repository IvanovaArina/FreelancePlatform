using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Strategy
{
    public class FixedPriceStrategy : IPaymentStrategy
    {
        public decimal CalculateCost(decimal baseAmount, int hours) => baseAmount;
    }
}

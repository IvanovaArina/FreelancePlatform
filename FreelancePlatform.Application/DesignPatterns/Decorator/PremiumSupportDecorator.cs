using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Decorator
{
    public class PremiumSupportDecorator : OrderDecorator
    {
        public PremiumSupportDecorator(IOrderComponent component) : base(component) { }
        public override string Description => _component.Description + ", Premium Support";
        public override decimal Cost => _component.Cost + 30m;

    }
}

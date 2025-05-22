using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Decorator
{
    public class UrgentOrderDecorator : OrderDecorator
    {
        public UrgentOrderDecorator(IOrderComponent component) : base(component)
        {
            
        }

        public override string Description => _component.Description + ", Urgent";
        public override decimal Cost => _component.Cost + 50m;
    }
}

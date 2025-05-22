using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Decorator
{
    public abstract class OrderDecorator : IOrderComponent
    {
        protected IOrderComponent _component;
        protected OrderDecorator(IOrderComponent component) => _component = component;
        
        public virtual string Description => _component.Description;

        public  virtual decimal Cost => _component.Cost;
    }
}

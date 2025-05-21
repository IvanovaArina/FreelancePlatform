using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Domain.Interfaces
{
    // Этот интерфейс используется для паттерна Decorator.
    // Он определяет свойства для описания и стоимости заказа,
    // позволяя добавлять дополнительные функции (например, срочность, премиум-поддержка)
    // через декораторы.
    public interface IOrderComponent
    {
        string Description { get; }
        decimal Cost { get; } // почему только гет?
    }
}

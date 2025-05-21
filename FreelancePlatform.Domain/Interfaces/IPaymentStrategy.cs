using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Domain.Interfaces
{
    // Этот интерфейс используется для паттерна Strategy.
    // Он определяет метод для расчёта стоимости заказа (например, фиксированная цена или почасовая оплата).
    // Позволяет гибко менять способ расчёта. - ?????????? HOW
    public interface IPaymentStrategy
    {
        decimal CalculateCost(decimal baseAmount, int hours);
    }
}

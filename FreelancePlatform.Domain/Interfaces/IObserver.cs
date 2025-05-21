using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Domain.Interfaces
{
    // Этот интерфейс используется для паттерна Observer.
    // Он определяет метод для получения уведомлений о событиях (например, создание заказа).
    // Позволяет подписчикам (например, email-уведомлениям) реагировать на изменения.
    public interface IObserver
    {
        void Update(string message);
    }
}

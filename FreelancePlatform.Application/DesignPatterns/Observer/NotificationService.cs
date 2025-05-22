using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Эти классы реализуют паттерн Observer (поведенческий паттерн).
// EmailNotifier — это наблюдатель, который получает уведомления
// (в данном случае выводит их в консоль). NotificationService управляет
// списком наблюдателей и отправляет им уведомления о событиях (например, о создании заказа).
// Почему Observer? Это позволяет уведомлять заинтересованные стороны о важных событиях.

namespace FreelancePlatform.Application.DesignPatterns.Observer
{
    public class NotificationService
    {
        private readonly List<IObserver> _observers = new List<IObserver> ();

        public void Subscribe (IObserver observer) => _observers.Add (observer);
        public void Unsubscribe (IObserver observer) => _observers.Remove (observer);
        public void Notify(string message)
        {
            foreach (var observer in _observers)
                observer.Update(message);
        }
    }

}

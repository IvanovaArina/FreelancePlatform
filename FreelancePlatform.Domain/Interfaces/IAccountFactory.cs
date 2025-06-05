using FreelancePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Domain.Interfaces
{
    // Этот интерфейс используется для реализации паттерна Abstract Factory.
    // Он определяет метод для создания пользователей с учётом их роли (клиент, фрилансер, администратор).
    // Мы добавили параметр name, чтобы передать имя пользователя.
    public interface IAccountFactory
    {
        User CreateUser(string email, string name);
        Dashboard CreateDashboard();
    }

   
    public class Dashboard
    {
        public string info { get; set; }
    }
}

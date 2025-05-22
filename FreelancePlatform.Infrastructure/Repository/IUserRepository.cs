using FreelancePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Отсутствие Update и Delete связано с тем, что обновление и удаление пользователей
//не предусмотрено в текущей версии платформы — пользователи могут быть только созданы,
//а их данные (например, email) изменяются через другие механизмы (например, профиль фрилансера).
//Получение по email добавлено для авторизации.
namespace FreelancePlatform.Infrastructure.Repository
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);   
        Task AddAsync(User user);
    }
}

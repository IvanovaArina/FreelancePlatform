using FreelancePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Этот интерфейс определяет метод для добавления профиля фрилансера.
// Отсутствие других методов (например, Get, Update, Delete) связано с тем,
// что профили создаются один раз и не удаляются — они могут обновляться через другие сервисы
// (например, ProfileService), а получение реализовано через связь с User.

namespace FreelancePlatform.Infrastructure.Repository
{
    public interface IProfileRepository
    {
        Task AddAsync(FreelancerProfile profile);
    }
}

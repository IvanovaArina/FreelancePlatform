using AutoMapper;
using FreelancePlatform.Application.DesignPatterns.AbstractFactory;
using FreelancePlatform.Application.DTOs;
using FreelancePlatform.Domain.Interfaces;
using FreelancePlatform.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Этот класс содержит бизнес-логику для управления пользователями: создание пользователей с заданной ролью.
// Использует паттерн Abstract Factory для создания пользователей и передаёт имя. Почему только создание?
// Обновление и удаление не предусмотрены, так как пользователи остаются в системе.

namespace FreelancePlatform.Application.Services
{
    public class UserService
    {
        private readonly Dictionary<string, IAccountFactory> _factories;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _factories = new Dictionary<string, IAccountFactory>
        {
            { "freelancer", new FreelancerFactory() },
            { "client", new ClientFactory() },
            { "admin", new AdminFactory() }
        };


        }

        public async Task<UserDto> CreateUserAsync(string role, string email, string name)
        {
            if (!_factories.TryGetValue(role.ToLower(), out var factory))
                throw new ArgumentException("Invalid role");

            var user = factory.CreateUser(email, name);
            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }

    }


}

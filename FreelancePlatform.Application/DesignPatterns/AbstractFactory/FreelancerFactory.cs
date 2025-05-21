using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.AbstractFactory
{
    public class FreelancerFactory : IAccountFactory
    {
        public User CreateUser(string email, string name)
        {
            return new User { Email = email, Name = name, Role = "Freelancer" };
        }
    }
}

using FreelancePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Observer
{
    public class EmailNotifier : IObserver
    {
        public void Update(string message) => Console.WriteLine($"Email: {message}");
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DesignPatterns.Adapter
{
    public class StripePaymentService
    {
        public bool Charge(decimal amount) => true; // imitation
    }
}

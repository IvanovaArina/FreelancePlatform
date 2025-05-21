using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Domain.Entities
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int Hours { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string FreelancerId { get; set; } = string.Empty;
        public bool IsUrgent {  get; set; }
        public bool HasPremiumSupport { get; set; }
    }
}

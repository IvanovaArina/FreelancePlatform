using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DTOs
{
    public class TransactionDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

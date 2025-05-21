using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Domain.Entities
{
    public class UserCredentials
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string PasswordHash {  get; set; } = string.Empty;
    }
}

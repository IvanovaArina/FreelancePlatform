using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Application.DTOs
{
    public class FreelancerProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<string> Portfolio { get; set; } = new List<string>();
        public List<string> Reviews { get; set; } = new List<string>();
        public List<string> Skills { get; set; } = new List<string>();
    }
}

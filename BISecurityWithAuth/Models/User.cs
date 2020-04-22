using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BISecurityWithAuth.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string LoginUserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}

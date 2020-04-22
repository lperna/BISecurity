using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BISecurityWithAuth.Models
{
    public class HomeViewModel
    {
        public User User { get; set; }
        public Customer Customer { get; set; }
        public List<WindowsCredential> WindowsCredentials { get; set; }
    }
}

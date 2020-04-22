using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BISecurityWithAuth.Models
{
    public class WindowsCredential {
        [Display(Name = "Windows User")]
        public string Username { get; set; }
        public List<WindowsGroup> GrantedGroups {get;set;}
    }
}

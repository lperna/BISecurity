using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace BISecurityWithAuth.Models
{
    public class Customer
    {
        [HiddenInput]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CredentialPrefix { get; set; }

}
}

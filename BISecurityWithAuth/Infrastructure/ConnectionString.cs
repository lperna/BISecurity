using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BISecurityWithAuth.Infrastructure
{
    public sealed class ConnectionString
    {
        public ConnectionString(string value) => Value = value;

        public string Value { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BISecurityWithAuth.Models;

namespace BISecurityWithAuth.Data
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(Guid id);

        int AddCustomer(Customer customer);

        int UpdateCustomer(Customer customer);
        int DeleteCustomerById(Guid id);

    }
}

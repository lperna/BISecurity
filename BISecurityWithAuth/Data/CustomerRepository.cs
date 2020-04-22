using BISecurityWithAuth.Infrastructure;
using BISecurityWithAuth.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BISecurityWithAuth.Data
{
    public class CustomerRepository : ICustomerRepository {
        private readonly ConnectionString _connectionString;

        public CustomerRepository(ConnectionString connectionString) {
            _connectionString = connectionString;
        }

        public IEnumerable<Customer> GetAllCustomers() {
            const string query = @"SELECT Id, Name,  CredentialPrefix  
                                FROM Customer;";

            using var conn = new SqlConnection(_connectionString.Value);
            var result = conn.Query<Customer>(query);
            return result;
        }

        public Customer GetCustomerById(Guid id) {
            const string query = @"SELECT Id, Name, CredentialPrefix  
                                FROM Customer
                                WHERE Id = @Id";
            using (var conn = new SqlConnection(_connectionString.Value)) {
                var result = conn.QueryFirstOrDefault<Customer>(query, new { Id = id });
                return result;
            }
        }

        public int AddCustomer(Customer customer) {
            const string query = @"INSERT INTO Customer ([Id],[Name], [CredentialPrefix]) VALUES(@Id, @Name, @LoginHost, @CredentialPrefix)";
            using (var conn = new SqlConnection(_connectionString.Value)) {
                var result = conn.Execute(
                    query,
                    new { Id = customer.Id, Name = customer.Name, CredentialPrefix = customer.CredentialPrefix });
                return result;
            }
        }

        public int UpdateCustomer(Customer customer) {
            const string query = @"UPDATE Customer Set [Name] =  @Name, [CredentialPrefix] = @CredentialPrefix WHERE Id = @Id";
            using (var conn = new SqlConnection(_connectionString.Value)) {
                var result = conn.Execute(
                    query,
                    new { Id = customer.Id, Name = customer.Name, CredentialPrefix = customer.CredentialPrefix });
                return result;
            }
        }

        public int DeleteCustomerById(Guid id) {
            const string query = @"DELETE  
                                FROM Customer
                                WHERE Id = @Id";
            using (var conn = new SqlConnection(_connectionString.Value)) {
                var result = conn.Execute(query, new { Id = id });
                return result;
            }
        }
    }
}

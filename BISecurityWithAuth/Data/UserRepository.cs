using BISecurityWithAuth.Infrastructure;
using BISecurityWithAuth.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BISecurityWithAuth.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectionString _connectionString;

        public UserRepository(ConnectionString connectionString) {
            _connectionString = connectionString;
        }
        public User GetUserById(Guid id) {
            const string query = @"SELECT [Id], [CustomerId], [LoginUserId] , [IsAdmin] 
                                FROM [User]
                                WHERE Id = @Id";
            try {
                using (var conn = new SqlConnection(_connectionString.Value)) {
                    var result = conn.QueryFirstOrDefault<User>(query, new { Id = id });
                    return result;
                }
            } catch (Exception e) {
                return null;
            }

        }
        public User GetUserByLoginUserId(string LoginUserId) {
            const string query = @"SELECT [Id], [CustomerId], [LoginUserId] , [IsAdmin] 
                                FROM [User]
                                WHERE [LoginUserId] = @LoginUserId";
            try {
                using (var conn = new SqlConnection(_connectionString.Value)) {
                    var result = conn.QueryFirstOrDefault<User>(query, new { LoginUserId = LoginUserId });
                    return result;
                }
            } catch (Exception e) {
                var msg = e.Message;
                return null;
            }

        }

        public int AddUser(User user) {
            const string query = @"INSERT INTO [User] ([Id],[CustomerId],[LoginUserId],[IsAdmin]) VALUES(@Id, @CustomerId, @LoginUserId, @IsAdmin)";
            try {
                using (var conn = new SqlConnection(_connectionString.Value)) {
                    var result = conn.Execute(
                        query,
                        new { Id = user.Id, CustomerId = user.CustomerId, LoginUserId = user.LoginUserId, IsAdmin = user.IsAdmin });
                    return result;
                }
            } catch (Exception e) {
                var msg = e.Message;
                throw;
            }
            
        }

        public int UpdateUser(User user) {
            const string query = @"UPDATE [User] Set [Id] =  @Id, [CustomerId] =  @CustomerId, [IsAdmin] = @IsAdmin WHERE Id = @Id";
            using (var conn = new SqlConnection(_connectionString.Value)) {
                var result = conn.Execute(
                    query,
                    new { Id = user.Id, CustomerId = user.CustomerId, LoginUserId = user.LoginUserId, IsAdmin = user.IsAdmin });
                return result;
            }
        }

        public int DeleteUserById(Guid id) {
            const string query = @"DELETE  
                                FROM [User]
                                WHERE Id = @Id";
            using (var conn = new SqlConnection(_connectionString.Value)) {
                var result = conn.Execute(query, new { Id = id });
                return result;
            }
        }
    }
}

using BISecurityWithAuth.Models;
using System;

namespace BISecurityWithAuth.Data
{
    public interface IUserRepository
    {
        int AddUser(User user);
        User GetUserById(Guid id);
        User GetUserByLoginUserId(string LoginUserid);
        int UpdateUser(User user);
        int DeleteUserById(Guid id);
    }
}
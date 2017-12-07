using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface IUserAccountRepository
    {
        IReadOnlyList<UserAccount> GetAllUsers();
        UserAccount GetUserById(Guid id);
        void CreateUser(UserAccount user);
        void EditUser(UserAccount user);
        void DeleteUser(UserAccount user);
    }
}
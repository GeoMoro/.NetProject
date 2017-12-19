using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface IUserStatusRepository
    {
        IReadOnlyList<UserStatus> GetUsersByLaboratory(Guid id);
        IReadOnlyList<UserStatus> GetAllUsers();
        UserStatus GetUserById(string id);
        void CreateUser(UserStatus user);
        void EditUser(UserStatus user);
        void DeleteUser(UserStatus user);
    }
}
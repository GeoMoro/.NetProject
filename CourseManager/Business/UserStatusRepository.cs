using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;

namespace Business
{
    public class UserStatusRepository : IUserStatusRepository
    {
        private readonly DatabaseContext _databaseService;

        public UserStatusRepository(DatabaseContext databaseService)
        {
            _databaseService = databaseService;
        }

        public IReadOnlyList<UserStatus> GetUsersByFactionId(Guid id)
        {
            return _databaseService.UserStatus.Where(user => user.FactionId == id).ToList();
        }

        public IReadOnlyList<UserStatus> GetAllUsers()
        {
            return _databaseService.UserStatus.ToList();
        }

        public UserStatus GetUserById(string id)
        {
            return _databaseService.UserStatus.SingleOrDefault(user => user.Id == id);
        }

        public void CreateUser(UserStatus user)
        {
            _databaseService.UserStatus.Add(user);

            _databaseService.SaveChanges();
        }

        public void EditUser(UserStatus user)
        {
            _databaseService.UserStatus.Update(user);

            _databaseService.SaveChanges();
        }

        public void DeleteUser(UserStatus user)
        {
            _databaseService.UserStatus.Remove(user);

            _databaseService.SaveChanges();
        }
    }
}
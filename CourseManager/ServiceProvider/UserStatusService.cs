using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServicesProvider
{
    public class UserStatusService : IUserStatusService
    {
        private readonly IUserStatusRepository _repository;

        public UserStatusService(IUserStatusRepository repository)
        {
            _repository = repository;
        }

        public IReadOnlyList<UserStatus> GetUsersByFactionId(Guid id)
        {
            return _repository.GetAllUsers().Where(user => user.FactionId == id).ToList();
        }

        //public IReadOnlyList<UserStatus> GetUsersByPresence(Guid id)
        //{
        //    return _repository.GetAllUsers().Where(user => user. == id).ToList();
        //}

        public UserStatus CreateAndReturnLatestUser(string id)//, double labMark, double kataMark, bool presence)
        {
            _repository.CreateUser(
                    UserStatus.CreateUsersStatus(
                        id
                    )
                );

            return _repository.GetAllUsers().LastOrDefault();
        }

        public void EditFaction(string id, Guid newFaction)
        {
            var searchedUser = _repository.GetUserById(id);
            searchedUser.FactionId = newFaction;
            _repository.EditUser(searchedUser);
        }

    }
}

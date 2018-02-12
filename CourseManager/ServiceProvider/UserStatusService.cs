using Data.Domain.Entities;
using Data.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.ServicesInterfaces;

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
        
        public UserStatus CreateAndReturnLatestUser(string id, Guid factionId)
        {
            _repository.CreateUser(
                    UserStatus.CreateUsersStatus(
                        id,
                        factionId,
                        false
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

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

        public IReadOnlyList<UserStatus> GetUsersByLaboratory(Guid id)
        {
            return _repository.GetUsersByLaboratory(id);
        }

        public UserStatus CreateAndReturnLatestUser(string id, Guid labId, double labMark, double kataMark, bool presence)
        {
            _repository.CreateUser(
                    UserStatus.CreateUsersStatus(
                        id,
                        labId,
                        labMark,
                        kataMark,
                        presence
                    )
                );

            return _repository.GetAllUsers().LastOrDefault();
        }

        public void EditLaboratory(string id, Guid newLabortory)
        {
            var searchedUser = _repository.GetUserById(id);
            searchedUser.LaboratoryId = newLabortory;
            _repository.EditUser(searchedUser);
        }

    }
}

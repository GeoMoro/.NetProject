using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesInterfaces;
using System;
using System.Collections.Generic;

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
    }
}

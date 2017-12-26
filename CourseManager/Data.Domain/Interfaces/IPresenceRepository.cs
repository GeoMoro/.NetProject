using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface IPresenceRepository
    {
        IReadOnlyList<Presence> GetAllPresences();
        Presence GetPresenceByName(string name);
        Presence GetPresenceById(Guid id);
        void CreatePresence(Presence presence);
        void UpdatePresence(Presence presence);
        void DeletePresence(Presence presence);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;

namespace Business
{
    public class PresenceRepository : IPresenceRepository
    {
        private readonly DatabaseContext _databaseContext;

        public PresenceRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Presence GetPresenceByName(string name)
        {
            return _databaseContext.Presences.SingleOrDefault(presence => presence.Name == name);
        }
        
        public IReadOnlyList<Presence> GetAllPresences()
        {
            return _databaseContext.Presences.ToList();
        }

        public Presence GetPresenceById(Guid id)
        {
            return _databaseContext.Presences.SingleOrDefault(presence => presence.Id == id);
        }

        public void CreatePresence(Presence presence)
        {
            _databaseContext.Presences.Add(presence);

            _databaseContext.SaveChanges();
        }

        public void UpdatePresence(Presence presence)
        {
            _databaseContext.Presences.Update(presence);

            _databaseContext.SaveChanges();
        }

        public void DeletePresence(Presence presence)
        {
            _databaseContext.Presences.Remove(presence);

            _databaseContext.SaveChanges();
        }
    }
}
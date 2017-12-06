using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;

namespace Business
{
    public class FactionRepository : IFactionRepository
    {
        private readonly DatabaseContext _databaseService;

        public FactionRepository(DatabaseContext databaseService)
        {
            _databaseService = databaseService;
        }

        public IReadOnlyList<Faction> GetAllFaction()
        {
            return _databaseService.Factions.ToList();
        }

        public Faction GetFactionById(Guid id)
        {
            return _databaseService.Factions.SingleOrDefault(faction => faction.Id == id);
        }

        public void CreateFaction(Faction faction)
        {
            _databaseService.Factions.Add(faction);

            _databaseService.SaveChanges();
        }

        public void UpdateFaction(Faction faction)
        {
            _databaseService.Factions.Update(faction);

            _databaseService.SaveChanges();
        }

        public void DeleteFaction(Faction faction)
        {
            _databaseService.Factions.Remove(faction);

            _databaseService.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface IFactionRepository
    {
        IReadOnlyList<Faction> GetAllFaction();
        Faction GetFactionById(Guid id);
        void CreateFaction(Faction faction);
        void UpdateFaction(Faction faction);
        void DeleteFaction(Faction faction);
    }
}
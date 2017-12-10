using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface IKataRepository
    {
        IReadOnlyList<Kata> GetAllKatas();
        Kata GetKataById(Guid id);
        void CreateKata(Kata kata);
        void EditKata(Kata kata);
        void DeleteKata(Kata kata);
    }
}

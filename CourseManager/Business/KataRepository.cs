using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;

namespace Business
{
    public class KataRepository : IKataRepository
    {
        private readonly DatabaseContext _databaseService;

        public KataRepository(DatabaseContext databaseService)
        {
            _databaseService = databaseService;
        }

        public IReadOnlyList<Kata> GetAllKatas()
        {
            return _databaseService.Katas.ToList();
        }

        public Kata GetKataById(Guid id)
        {
            return _databaseService.Katas.SingleOrDefault(kata => kata.Id == id);
        }

        public void CreateKata(Kata kata)
        {
            _databaseService.Katas.Add(kata);

            _databaseService.SaveChanges();
        }

        public void EditKata(Kata kata)
        {
            _databaseService.Katas.Update(kata);

            _databaseService.SaveChanges();
        }

        public void DeleteKata(Kata kata)
        {
            _databaseService.Katas.Remove(kata);

            _databaseService.SaveChanges();
        }
    }
}

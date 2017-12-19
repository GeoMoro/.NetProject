using Data.Domain.Interfaces;
using Data.Domain.Interfaces.ServicesNews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Data.Domain.Entities;
using Data.Persistance;

namespace RecordServiceProvider
{
    public class RecordService : IRecordService {
        private readonly INewsRepository _repository;
        public int Count;
        private readonly DatabaseContext _databaseContext;

        public RecordService(DatabaseContext databaseContext, INewsRepository repository) {
            _databaseContext = databaseContext;

            _repository = repository;
        }
        
        public IReadOnlyList<News> GetFirstFive()
        {
            Count = 5;
            return _databaseContext.News.OrderByDescending(d => d.CreatedAtDate).Take(5).ToList();
        }

        public IReadOnlyList<News> GetNextFiveOrTheRest(int Count)
        {
            return _databaseContext.News.OrderByDescending(d => d.CreatedAtDate).Skip(Count).Take(5).ToList();
        }

        public int GetNumberOfElements()
        {
            return _repository.GetAllNews().Count;
        }
    }
}

using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;
using System.Collections.Generic;
using Business.ServicesInterfaces;

namespace RecordServiceProvider
{
    public class RecordService : IRecordService
    {
        private readonly INewsRepository _repository;
        public int Count;
        private readonly DatabaseContext _databaseContext;

        public RecordService(DatabaseContext databaseContext, INewsRepository repository)
        {
            _databaseContext = databaseContext;

            _repository = repository;
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

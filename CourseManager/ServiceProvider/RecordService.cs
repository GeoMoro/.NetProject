using System;
using System.Collections.Generic;
using System.Linq;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;

namespace ServicesProvider
{
    public class RecordService : IRecordService
    {
        private readonly INewsRepository _repository;
        public int Count;
        private readonly IDatabaseContext _databaseContext;

        public RecordService(IDatabaseContext databaseContext, INewsRepository repository)
        {
            _databaseContext = databaseContext;

            _repository = repository;
        }

        public void Create(string createdBy, NewsCreateModel newsCreateModel)
        {
            _repository.CreateNews(
            News.CreateNews(
                newsCreateModel.Title,
                newsCreateModel.Description,
                createdBy
                )
            );
        }

        public void Update(News news)
        {
            _repository.UpdateNews(news);
        }

        public void Delete(News news)
        {
            _repository.DeleteNews(news);
        }

        public News GetNewsById(Guid? id)
        {
            return _repository.GetNewsById(id.Value);
        }

        public bool NewsExists(Guid id)
        {
            return _repository.GetAllNews().Any(e => e.Id == id);
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

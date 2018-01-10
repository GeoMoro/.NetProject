using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;

namespace Business
{
    public class NewsRepository : INewsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public NewsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IReadOnlyList<News> GetAllNews()
        {
            return _databaseContext.News.ToList();
        }

        public News GetNewsById(Guid id)
        {
            return _databaseContext.News.SingleOrDefault(news => news.Id == id);
        }

        public void CreateNews(News news)
        {
            _databaseContext.News.Add(news);
            _databaseContext.SaveChanges();
        }

        public void UpdateNews(News news)
        {
            _databaseContext.News.Update(news);
            _databaseContext.SaveChanges();
        }

        public void DeleteNews(News news)
        {
            _databaseContext.News.Remove(news);
            _databaseContext.SaveChanges();
        }
    }
}
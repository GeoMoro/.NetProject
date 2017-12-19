using System;
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface INewsRepository
    {
        IReadOnlyList<News> GetAllNews();
        News GetNewsById(Guid id);
        void CreateNews(News news);
        void UpdateNews(News news);
        void DeleteNews(News news);
    }
}
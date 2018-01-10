using System;
using System.Collections.Generic;
using Business.ServicesInterfaces.Models;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IRecordService
    {
        IReadOnlyList<News> GetNextFiveOrTheRest(int count);
        int GetNumberOfElements();
        void Create(string createdBy, NewsCreateModel newsCreateModel);
        News GetNewsById(Guid? id);
        bool NewsExists(Guid id);
        void Update(News newsEdited);
        void Delete(News news);
    }
}
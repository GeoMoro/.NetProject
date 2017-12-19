using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces.ServicesNews
{
    public interface IRecordService
    {
        IReadOnlyList<News> GetFirstFive();
        IReadOnlyList<News> GetNextFiveOrTheRest(int Count);
        int GetNumberOfElements();
    }
}
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface IRecordService
    {
        IReadOnlyList<News> GetNextFiveOrTheRest(int Count);
        int GetNumberOfElements();
    }
}
using System.Collections.Generic;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IRecordService
    {
        IReadOnlyList<News> GetNextFiveOrTheRest(int count);
        int GetNumberOfElements();
    }
}
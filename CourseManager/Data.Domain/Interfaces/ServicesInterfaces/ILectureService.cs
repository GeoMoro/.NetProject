using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Interfaces
{
    public interface ILectureService
    {
        List<string> GetFiles(Guid id);
    }
}

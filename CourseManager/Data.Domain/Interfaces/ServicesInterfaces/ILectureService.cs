using System;
using System.Collections.Generic;
using System.IO;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface ILectureService
    {
        List<string> GetFiles(Guid id);
        List<string> GetFilesBasedOnDetails(string title, string description);
        void DeleteFilesForGivenId(Guid id);
        void DeleteSpecificFiles(string fileName, Guid? givenId);
        Stream Download(Guid lectureId);
    }
}

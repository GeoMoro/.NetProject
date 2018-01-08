using System;
using System.Collections.Generic;

namespace Business.ServicesInterfaces
{
    public interface IKataService
    {
        List<string> GetFiles(Guid id);
        List<string> GetFilesBasedOnDetails(string title, string description);
        void DeleteFilesForGivenId(Guid id);
        void DeleteSpecificFiles(string fileName, Guid? givenId);
    }
}

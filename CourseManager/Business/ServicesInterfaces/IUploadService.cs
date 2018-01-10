using System.Collections.Generic;

namespace Business.ServicesInterfaces
{
    public interface IUploadService
    {
        List<string> GetFiles(string type, string seminar, string name);
        List<string> GetAllFiles(string type, string seminar);
    }
}

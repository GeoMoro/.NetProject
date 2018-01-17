using Business.ServicesInterfaces.Models.UploadsViewModels;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Business.ServicesInterfaces
{
    public interface IUploadService
    {
        List<string> GetFiles(string type, string seminar, string name);
        List<string> GetAllFiles(string type, string seminar);
        Task CreateUploads(string userGroup, string userFirstName, string userLastName, UploadsCreateModel uploadCreateModel);
        Stream DownloadFile(string seminarName, string group, string seminarNumber, string fileName);
    }
}

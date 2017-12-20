using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.Services
{
    public interface IUploadService
    {
        List<string> GetFiles(string type, string seminar, string name);
        List<string> GetAllFiles(string type, string seminar);
    }
}

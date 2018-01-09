using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Business.ServicesInterfaces.Models.KataViewModels;
using Data.Domain.Entities;

namespace Business.ServicesInterfaces
{
    public interface IKataService
    {
        List<string> GetFiles(Guid id);
        List<string> GetFilesBasedOnDetails(string title, string description);
        Task CreateKata(KataCreateModel kataToBeCreated);
        Kata GetKataInfoByDetails(string title, string description);
        void DeleteFilesForGivenId(Guid id);
        Stream SearchKata(Guid kataId, string fileName);
        void DeleteFile(string fileName, Guid? givenId);
        Kata GetKataById(Guid idValue);
        IReadOnlyList<Kata> GetAllKatas();
        bool GetAll(Guid id);
        Task Edit(Guid id, Kata kataEdited, KataEditModel kataModel);
        void DeleteKata(Kata kata);
        void DeleFromPath(Guid id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceTests
{
    [TestClass]
    public class KataTests
    {
        [TestMethod]
        public void MethodName_Input_ExpectedBehaviour()
        {
            // Arrange

            // Act

            // Assert
        }

        #region Repository Mock Class

        public class KataRepositoryMock : IKataRepository
        {
            public List<Kata> Katas;

            public KataRepositoryMock()
            {
                Katas = new List<Kata>
                {
                    Kata.CreateKata("Title1", "Description1"),
                    Kata.CreateKata("Title2", "Description2"),
                    Kata.CreateKata("Title3", "Description3"),
                    Kata.CreateKata("Title4", "Description4"),
                    Kata.CreateKata("Title5", "Description5")
                };

                Katas[0].Id = new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986");
                Katas[1].Id = new Guid("d3ba2f3b-2a1f-4175-bc05-a0a89a14942d");
            }

            public IReadOnlyList<Kata> GetAllKatas()
            {
                throw new NotImplementedException();
            }

            public Kata GetKataById(Guid id)
            {
                throw new NotImplementedException();
            }

            public Kata GetKataInfoByDetails(string title, string description)
            {
                throw new NotImplementedException();
            }

            public void CreateKata(Kata kata)
            {
                throw new NotImplementedException();
            }

            public void EditKata(Kata kata)
            {
                throw new NotImplementedException();
            }

            public void DeleteKata(Kata kata)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}

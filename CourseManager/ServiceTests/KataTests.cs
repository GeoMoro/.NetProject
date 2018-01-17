using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.ServicesInterfaces.Models.KataViewModels;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using ServicesProvider;


namespace ServiceTests
{
    [TestClass]
    public class KataTests
    {

        public KataRepositoryMock Mock = new KataRepositoryMock();

        

        [TestMethod]
        public async Task CreateKata_WhenCalled_ExpectToCreateAKata()
        {
            // Arrange
            var sut = CreateSut();
            var mockFile = new Mock<IFormFile>();

            mockFile.Setup(_ => _.FileName).Returns("TestFile.txt");
            var kataTobeCreate = new KataCreateModel
            {
                Title = "TitleX",
                Description = "DescriptionX",
                File = new List<IFormFile>
                {
                    mockFile.Object
                }
            };

            // Act
            await sut.CreateKata(kataTobeCreate);

            // Assert
            Assert.AreEqual(Mock.Katas.Count, 6);
        }


        [TestMethod]
        public void GetFile_GivenAKataID_ExpectToReturnThatKataFiles()
        {
            // Arrange
            var sut = CreateSut();
            var folder = Guid.NewGuid();
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Katas\\" + folder;

            CreateFile(searchedPath, "MyTest.txt");

            // Act
            var files = sut.GetFiles(folder);

            // Assert
            Assert.AreEqual(files[0], "MyTest.txt");

            DeleteFile(searchedPath);
        }



        [TestMethod]
        public void GetFilesBasedOnDetails_GivendDetails_GetTheProperFiles()
        {
            // Arrange
            var sut = CreateSut();
            var idValue = new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986");
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Katas\\" + idValue;

            CreateFile(searchedPath, "MyTest.txt");

            // Act
            var files = sut.GetFilesBasedOnDetails("Title1", "Description1");

            // Assert
            Assert.AreEqual(files[0], "MyTest.txt");

            DeleteFile(searchedPath);
        }


        [TestMethod]
        public void DeleteFilesForGivenId_GivendDetails_DeleteTheProperFiles()
        {
            // Arrange
            var sut = CreateSut();
            var idValue = new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986");
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Katas\\" + idValue;

            CreateFile(searchedPath, "MyTest.txt");

            // Act
            sut.DeleteFilesForGivenId(idValue);

            // Assert
            Assert.AreEqual(Directory.Exists(searchedPath), false);
        }

        [TestMethod]
        public void DeleteFile_GivendDetails_DeleteTheProperFiles()
        {
            // Arrange
            var sut = CreateSut();
            var idValue = new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986");
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Katas\\" + idValue;

            CreateFile(searchedPath, "MyTest.txt");

            // Act
            sut.DeleteFile("MyTest.txt", idValue);

            // Assert
            Assert.AreEqual(File.Exists(searchedPath + "/MyTest.txt"), false);
        }

        [TestMethod]
        public void SearchKata_GivendDetails_SearchTheProperKatas()
        {
            // Arrange
            var sut = CreateSut();
            var idValue = new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986");
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Katas\\" + idValue;

            CreateFile(searchedPath, "MyTest.txt");

            // Act
            var file = sut.SearchKata(idValue, "MyTest.txt");

            // Assert
            Assert.AreEqual(file.CanRead, true);
        }

        [TestMethod]
        public void DeleteKata_GivendDetails_EliminateTheKata()
        {
            // Arrange
            var sut = CreateSut();
            var kata = Kata.CreateKata("B-Bam", "Description");
            Mock.Katas.Add(kata);

            // Act
            sut.DeleteKata(kata);

            // Assert
            Assert.AreEqual(Mock.Katas.Count, 5);
        }


        [TestMethod]
        public async Task EditKata_WhenCalled_ExpectToEditAKata()
        {
            // Arrange
            var sut = CreateSut();
            var idValue = new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986");
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(_ => _.FileName).Returns("TestFile.txt");

            var kataTobeEdited = new KataEditModel
            {
                Title = "NewTitle",
                Description = "DescriptionA",
                File = new List<IFormFile>
                {
                    mockFile.Object
                }
            };


            Mock.Katas[0].Description = kataTobeEdited.Description;
            Mock.Katas[0].Title = kataTobeEdited.Title;

            // Act
            await sut.Edit(idValue, Mock.Katas[0], kataTobeEdited);

            // Assert
            Assert.AreEqual(Mock.Katas[0].Description, "DescriptionA");
        }


        [TestMethod]
        public void GetKataById_GivenAnId_GetTheKataWithThatId()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var searchedKata = sut.GetKataById(new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986"));

            // Assert
            Assert.AreEqual(Mock.Katas[0], searchedKata);
        }


        [TestMethod]
        public void GetAllKatas_WhenCalled_ExpectToGetAllKatas()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var allKatas = sut.GetAllKatas();

            // Assert
            Assert.AreEqual(allKatas.Count, 5);
        }


        private KataService CreateSut()
        {
            var mockEnvironment = new Mock<IHostingEnvironment>();
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns(Directory.GetCurrentDirectory() + "\\wwwroot");
            return new KataService(Mock, mockEnvironment.Object);
        }



        private void CreateFile(string path, string file)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = path + "/" + file;
            if (!File.Exists(fileName))
            {
                using (StreamWriter streamwrite = File.CreateText(fileName))
                {
                    streamwrite.WriteLine("A file created for testing!");
                }
            }
        }

        private void DeleteFile(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
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
                return Katas;
            }

            public Kata GetKataById(Guid id)
            {
                return Katas.SingleOrDefault(kata => kata.Id.Equals(id));
            }

            public Kata GetKataInfoByDetails(string title, string description)
            {
                return Katas.SingleOrDefault(kata => kata.Title == title && kata.Description == description);
            }

            public void CreateKata(Kata kata)
            {
                Katas.Add(kata);
            }

            public void EditKata(Kata kata)
            {
                var kataToBeUpdate = GetKataById(kata.Id);
                kataToBeUpdate.Title = kata.Title;
                kataToBeUpdate.Description = kata.Description;
            }

            public void DeleteKata(Kata kata)
            {
                Katas.Remove(kata);
            }
        }

        #endregion

    }
}
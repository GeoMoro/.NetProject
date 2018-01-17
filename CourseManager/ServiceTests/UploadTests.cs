using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Business.ServicesInterfaces;
using Business.ServicesInterfaces.Models.UploadsViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using Moq;
using ServicesProvider;

namespace ServiceTests
{
    [TestClass]
    public class UploadTests
    {
        [TestMethod]
        public async Task CreateUpload_WhenCalled_ExpectToCreateAnUpload()
        {
            // Arrange
            var sut = CreateSut();
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(_ => _.FileName).Returns("TestFile.txt");

            var uploadCreateModel = new UploadsCreateModel
            {
                Seminar = "2",
                Type = "Laboratory",
                File = mockFile.Object
            };

            var path = Directory.GetCurrentDirectory() + "\\wwwroot\\Uploads\\" + uploadCreateModel.Type + "\\" + uploadCreateModel.Seminar;
            // Act
            await sut.CreateUploads("B3", "Test", "Test", uploadCreateModel);

            // Assert
            Assert.AreEqual(Directory.Exists(path), true);
        }

        [TestMethod]
        public void GetFiles_WhenCalled_ExpectToReturnThatUploadFiles()
        {
            // Arrange
            var sut = CreateSut();
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Uploads\\Seminar\\2";
     
            CreateFile(searchedPath, "MyTest.txt");

            // Act
            var files = sut.GetFiles("Seminar", "2", "MyTest");

            // Assert
            Assert.AreEqual(files[0], "MyTest.txt");

            DeleteFile(searchedPath);
        }

        [TestMethod]
        public void GetAllFiles_WhenCalled_ExpectToReturnThatUploadFiles()
        {
            // Arrange
            var sut = CreateSut();
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Uploads\\Seminar\\2";

            CreateFile(searchedPath, "MyTest.txt");

            // Act
            var files = sut.GetAllFiles("Seminar", "2");

            // Assert
            Assert.AreEqual(files[0], "MyTest.txt");

            DeleteFile(searchedPath);
        }

        private UploadService CreateSut()
        {
            var mockEnvironment = new Mock<IHostingEnvironment>();
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns(Directory.GetCurrentDirectory() + "\\wwwroot");
            return new UploadService(mockEnvironment.Object);
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
    }
}

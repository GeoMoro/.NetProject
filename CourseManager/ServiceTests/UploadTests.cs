using System.IO;
using System.Threading.Tasks;
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
                Teacher = "Best teacher",
                Week = "2",
                Type = "Laboratory",
                File = mockFile.Object
            };

            var path = Directory.GetCurrentDirectory() + "\\wwwroot\\Uploads\\" + uploadCreateModel.Teacher + "\\" + uploadCreateModel.Type + "\\" + uploadCreateModel.Week;
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
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Uploads\\BestTeacher\\Laboratory\\2";
     
            CreateFile(searchedPath, "MyTest.txt");

            // Act
            var files = sut.GetFiles("Laboratory", "2", "MyTest", "BestTeacher");

            // Assert
            Assert.AreEqual(files[0], "MyTest.txt");

            DeleteFile(searchedPath);
        }

        [TestMethod]
        public void GetAllFiles_WhenCalled_ExpectToReturnThatUploadFiles()
        {
            // Arrange
            var sut = CreateSut();
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Uploads\\BestTeacher\\Laboratory\\2";

            CreateFile(searchedPath, "MyTest.txt");

            // Act
            var files = sut.GetAllFiles("Laboratory", "2", "BestTeacher");

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

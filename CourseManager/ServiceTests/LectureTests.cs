using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.ServicesInterfaces.Models.LectureViewModels;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicesProvider;

namespace ServiceTests
{
    [TestClass]
    public class LectureTests
    {
        public LectureRepositoryMock Mock = new LectureRepositoryMock();

        [TestMethod]
        public async Task CreateLecture_WhenCalled_ExpectToCreateALecture()
        {
            // Arrange
            var sut = CreateSut();
            var mockFile = new Mock<IFormFile>();

            mockFile.Setup(_ => _.FileName).Returns("TestFile.txt");
            var lectureTobeCreate = new LectureCreateModel
            {
                Title = "TitleX",
                Description = "DescriptionX",
                File = new List<IFormFile>
                {
                    mockFile.Object
                }
            };
            
            // Act
            await sut.CreateLecture(lectureTobeCreate);

            // Assert
            Assert.AreEqual(Mock.Lectures.Count, 6);
        }

        [TestMethod]
        public void GetFile_GivenALectureID_ExpectToReturnThatLectureFiles()
        {
            // Arrange
            var sut = CreateSut();
            var folder = Guid.NewGuid();
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Lectures\\" + folder;
            
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
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Lectures\\" + idValue;

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
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Lectures\\" + idValue;
           
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
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Lectures\\" + idValue;

            CreateFile(searchedPath, "MyTest.txt");

            // Act
            sut.DeleteFile("MyTest.txt", idValue);

            // Assert
            Assert.AreEqual(File.Exists(searchedPath + "/MyTest.txt"), false);
        }

        [TestMethod]
        public void SearchLecture_GivendDetails_SearchTheProperLectures()
        {
            // Arrange
            var sut = CreateSut();
            var idValue = new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986");
            var searchedPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Lectures\\" + idValue;

            CreateFile(searchedPath, "MyTest.txt");

            // Act
            var file = sut.SearchLecture(idValue, "MyTest.txt");

            // Assert
            Assert.AreEqual(file.CanRead, true);
        }

        [TestMethod]
        public void DeleteLecture_GivendDetails_EliminateTheLecture()
        {
            // Arrange
            var sut = CreateSut();
            var lecture = Lecture.CreateLecture("B-Bam", "Description");
            Mock.Lectures.Add(lecture);

            // Act
            sut.DeleteLecture(lecture);

            // Assert
            Assert.AreEqual(Mock.Lectures.Count, 5);
        }
        
        [TestMethod]
        public async Task EditLecture_WhenCalled_ExpectToEditALecture()
        {
            // Arrange
            var sut = CreateSut();
            var idValue = new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986");
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(_ => _.FileName).Returns("TestFile.txt");

            var lectureTobeEdited = new LectureEditModel
            {
                Title = "NewTitle",
                Description = "DescriptionA",
                File = new List<IFormFile>
                {
                    mockFile.Object
                }
            };


            Mock.Lectures[0].Description = lectureTobeEdited.Description;
            Mock.Lectures[0].Title = lectureTobeEdited.Title;

            // Act
            await sut.Edit(idValue, Mock.Lectures[0], lectureTobeEdited);

            // Assert
            Assert.AreEqual(Mock.Lectures[0].Description, "DescriptionA");
        }
        
        [TestMethod]
        public void GetLectureById_GivenAnId_GetTheLectureWithThatId()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var searchedLecture = sut.GetLectureById(new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986"));

            // Assert
            Assert.AreEqual(Mock.Lectures[0], searchedLecture);
        }

        [TestMethod]
        public void GetAllLectures_WhenCalled_ExpectToGetAllLectures()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var allLectures = sut.GetAllLectures();

            // Assert
            Assert.AreEqual(allLectures.Count, 5);
        }

        [TestMethod]
        public void CheckIfLecturesExists_GivenAnId_ShouldReturnTrueIfALectureWithThatIdExists()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var exists = sut.CheckIfLecturesExists(new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986"));

            // Assert
            Assert.AreEqual(exists, true);
        }

        private LectureService CreateSut()
        {
            var mockEnvironment = new Mock<IHostingEnvironment>();
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns(Directory.GetCurrentDirectory() + "\\wwwroot");
            return new LectureService(Mock, mockEnvironment.Object);
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

        public class LectureRepositoryMock : ILectureRepository
        {
            public List<Lecture> Lectures;

            public LectureRepositoryMock()
            {
                Lectures = new List<Lecture>
                {
                    Lecture.CreateLecture("Title1", "Description1"),
                    Lecture.CreateLecture("Title2", "Description2"),
                    Lecture.CreateLecture("Title3", "Description3"),
                    Lecture.CreateLecture("Title4", "Description4"),
                    Lecture.CreateLecture("Title5", "Description5")
                };

                Lectures[0].Id = new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986");
            }

            public IReadOnlyList<Lecture> GetAllLectures()
            {
                return Lectures;
            }

            public Lecture GetLectureById(Guid id)
            {
                return Lectures.SingleOrDefault(lecture => lecture.Id.Equals(id));
            }

            public Lecture GetLectureInfoByDetails(string title, string description)
            {
                return Lectures.SingleOrDefault(lecture => lecture.Title == title && lecture.Description == description);
            }

            public void CreateLecture(Lecture lecture)
            {
                Lectures.Add(lecture);
            }

            public void EditLecture(Lecture lecture)
            {
                var lectureToBeUpdate = GetLectureById(lecture.Id);

                lectureToBeUpdate.Title = lecture.Title;
                lectureToBeUpdate.Description = lecture.Description;
            }

            public void DeleteLecture(Lecture lecture)
            {
                Lectures.Remove(lecture);
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.ServicesInterfaces.Models.LectureViewModels;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var lectureTobeCreate = new LectureCreateModel
            {
                Title = "TitleX",
                Description = "DescriptionX"
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

            // TODO: create some test files for the first lecture in MockRepository, make a folder named TestFiles or sth like this
            // TODO: ask Alex Corfu for more information about how this work exactly or inspect the code to find out yourself

            // Act
            var files = sut.GetFiles(new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986"));

            // Assert

        }

        [TestMethod]
        public void GetFilesBasedOnDetails_GivendDetails_GetTheProperFiles()
        {
            // Arrange

            // Act

            // Assert

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

        private LectureService CreateSut()
        {
            return new LectureService(Mock, new HostingEnvironment());
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

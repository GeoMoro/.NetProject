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
            var sut = new LectureService(Mock, new HostingEnvironment());
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
    }
}

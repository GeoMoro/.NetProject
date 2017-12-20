using System.Collections.Generic;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Presentation.Controllers;

namespace PresentationTests
{
    [TestClass]
    public class LecturesControllerTests
    {
        private static LecturesController CreateSut()
        {
            IHostingEnvironment nenv = new HostingEnvironment();
            var mock = new Mock<ILectureRepository>();

            mock.Setup(lectRep => lectRep.GetAllLectures()).Returns(
                new List<Lecture>
                {
                    Lecture.CreateLecture("Title 1", "Description 1"),
                    Lecture.CreateLecture("Title 2", "Description 2"),
                    Lecture.CreateLecture("Title 3", "Description 3"),
                    Lecture.CreateLecture("Title 4", "Description 4"),
                    Lecture.CreateLecture("Title 5", "Description 5"),
                }.AsReadOnly());

            return new LecturesController(mock.Object, nenv);
        }

        [TestMethod]
        public void Index_ShouldReturnAViewWithLectures()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = (IReadOnlyList<Lecture>) sut.Index().Model;

            // Assert
            Assert.IsInstanceOfType(result, typeof(IReadOnlyList<Lecture>));
        }
    }
}
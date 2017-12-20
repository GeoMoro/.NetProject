using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Presentation.Controllers;
using Presentation.Models.LectureViewModels;

namespace PresentationTests
{
    [TestClass]
    public class LecturesControllerTests
    {
        private readonly IHostingEnvironment _nenv = new HostingEnvironment();
        private readonly Mock<ILectureRepository> _mock = new Mock<ILectureRepository>();

        [TestMethod]
        public void Index_ShouldReturnAViewWithLectures()
        {
            // Arrange
            _mock.Setup(lectRep => lectRep.GetAllLectures()).Returns(
                new List<Lecture>
                {
                    Lecture.CreateLecture("Title 1", "Description 1"),
                    Lecture.CreateLecture("Title 2", "Description 2"),
                    Lecture.CreateLecture("Title 3", "Description 3"),
                    Lecture.CreateLecture("Title 4", "Description 4"),
                    Lecture.CreateLecture("Title 5", "Description 5"),
                }.AsReadOnly());

            var sut = new LecturesController(_mock.Object, _nenv);

            // Act
            //var result = (IReadOnlyList<Lecture>) sut.Index().Model;
            var result = (IReadOnlyList<Lecture>)sut.Index().Model;

            // Assert
            Assert.IsInstanceOfType(result, typeof(IReadOnlyList<Lecture>));
        }

        [TestMethod]
        public void Create_ShouldReturnAView()
        {
            // Arrange
            var sut = new LecturesController(_mock.Object, _nenv);

            // Act
            var result = sut.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        //[TestMethod]
        //public async Task Create_GivenAnInvalidLectureCreateModel_ShouldReturnARedirectToActionResult()
        //{
        //    // Arrange
        //    var sut = new LecturesController(_mock.Object, _nenv);
        //    var testModel = new LectureCreateModel();

        //    // Act
        //    var result = await sut.Create(testModel);

        //    // Assert
        //    Assert.IsInstanceOfType(result, typeof(ViewResult));
        //}

        [TestMethod]
        public async Task Create_GivenAValidLectureCreateModel_ShouldReturnARedirectToActionResult()
        {
            // Arrange
            var sut = new LecturesController(_mock.Object, _nenv);

            var testModel = new LectureCreateModel
            {
                Description = "Description",
                File = new List<IFormFile>(),
                Title = "Title"
            };

            // Act
            var result = await sut.Create(testModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public void Edit_GivenNullId_ShouldReturnNotFound()
        {
            // Arrange
            var sut = new LecturesController(_mock.Object, _nenv);

            // Act
            var result = sut.Edit(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
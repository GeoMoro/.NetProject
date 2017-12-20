using System;
using System.Collections.Generic;
using System.Text;
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
using Presentation.Models.KataViewModels;


namespace PresentationTests
{
        [TestClass]
        public class KatasControllerTests
        {
            private readonly IHostingEnvironment _nenv = new HostingEnvironment();
            private readonly Mock<IKataRepository> _mock = new Mock<IKataRepository>();

            [TestMethod]
            public void Index_ShouldReturnAViewWithKatas()
            {
                // Arrange
                _mock.Setup(kataRep => kataRep.GetAllKatas()).Returns(
                    new List<Kata>
                    {
                    Kata.CreateKata("Title 1", "Description 1"),
                    Kata.CreateKata("Title 2", "Description 2"),
                    Kata.CreateKata("Title 3", "Description 3"),
                    Kata.CreateKata("Title 4", "Description 4"),
                    Kata.CreateKata("Title 5", "Description 5"),
                    }.AsReadOnly());

                var sut = new KatasController(_mock.Object, _nenv);

                // Act
                //var result = (IReadOnlyList<Lecture>) sut.Index().Model;
                var result = (IReadOnlyList<Kata>)sut.Index().Model;

                // Assert
                Assert.IsInstanceOfType(result, typeof(IReadOnlyList<Kata>));
            }


            [TestMethod]
            public void Create_ShouldReturnAView()
            {
                // Arrange
                var sut = new KatasController(_mock.Object, _nenv);

                // Act
                var result = sut.Create();

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }

            [TestMethod]
            public async Task Create_GivenAValidKataCreateModel_ShouldReturnARedirectToActionResult()
            {
                // Arrange
                var sut = new KatasController(_mock.Object, _nenv);

                var testModel = new KataCreateModel
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
                var sut = new KatasController(_mock.Object, _nenv);

                // Act
                var result = sut.Edit(null);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public void Edit_GivenInexistentKataId_ShouldReturnNotFound()
            {
                // Arrange
                _mock.Setup(kataRep => kataRep.GetKataById(new Guid()))
                    .Returns(() => null);

                var sut = new KatasController(_mock.Object, _nenv);

                // Act
                var result = sut.Edit(new Guid());

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

     
        }


        }
}

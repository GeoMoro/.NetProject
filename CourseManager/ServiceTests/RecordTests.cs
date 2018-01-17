using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Business.ServicesInterfaces.Models;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicesProvider;

namespace ServiceTests
{
    [TestClass]
    public class RecordTests
    {
        public RecordRepositoryMock Mock = new RecordRepositoryMock();

        #region Repository Mock Class

        public class RecordRepositoryMock : INewsRepository
        {
            public List<News> Newses;

            public RecordRepositoryMock()
            {
                Newses = new List<News>
                {
                    News.CreateNews("examen","data","Olariu"),
                    News.CreateNews("curs","prezenta obligatorie","Olariu"),
                    News.CreateNews("laborator","prezenta obligatorie","Olariu"),
                    News.CreateNews("laborator","Teme","Olariu"),
                    News.CreateNews("proiect","sgfg","Olariu"),


                };

                Newses[0].Id = new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986");
            }

            public IReadOnlyList<News> GetAllNews()
            {
                return Newses;
            }

            public News GetNewsById(Guid id)
            {
                return Newses.SingleOrDefault(news => news.Id.Equals(id));
            }

            public void CreateNews(News news)
            {
                Newses.Add(news);
            }

            public void UpdateNews(News news)
            {
                var newsToBeUpdate = GetNewsById(news.Id);
                newsToBeUpdate.Title = "Good title";
                newsToBeUpdate.Description = "descript";
            }

            public void DeleteNews(News news)
            {
                Newses.Remove(news);
            }

        }

        #endregion
        
        [TestMethod]
        public void GetNumberOfElements_WhenCalled_ExpectedToGetNumberOfNews()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            var allNews = sut.GetNumberOfElements();

            //Assert
            Assert.AreEqual(allNews, 5);
        }

        [TestMethod]
        public void Create_WhenCalled_ExpectedToCreateNews()
        {
            //Arrange
            var sut = CreateSut();
            var newsCreateModel = new NewsCreateModel
            {
                Title = "for tests",
                Description = "Sure!"
            };

            //Act
           sut.Create("A user made for testing!", newsCreateModel);

            //Assert
            Assert.AreEqual(Mock.Newses.Count, 6);
        }

        [TestMethod]
        public void CheckIfNewsExists_WhenCalled_ExpectedToReturnTrue()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            var result = sut.GetNewsById(Mock.Newses[0].Id);

            //Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void Delete_WhenCalled_ExpectedToDeleteNews()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            sut.Delete(Mock.Newses[0]);

            //Assert
            Assert.AreEqual(Mock.Newses.Count, 4);
        }

        [TestMethod]
        public void Update_WhenCalled_ExpectedToUpdateNews()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            sut.Update(Mock.Newses[0]);

            //Assert
            Assert.AreEqual(Mock.Newses[0].Title, "Good title");
        }

        private RecordService CreateSut()
        {
            var mockDb = new Mock<IDatabaseContext>();

            return new RecordService(mockDb.Object, Mock);
        }


    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.ServicesInterfaces.Models;
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
    public class RecordTests
    {

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

                Newses[0].Id=new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986"); 
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
                newsToBeUpdate.Title = news.Title;
                newsToBeUpdate.Description = news.Description;
            }

            public void DeleteNews(News news)
            {
                Newses.Remove(news);
            }

        }

        #endregion






        [TestMethod]
        public void GetNextFiveOrTheRest_WhenCalled_ExpectToGetNextFiveNews()
        {
            //Arrange

            var sut = CreateSut();
            var count = 0;
            
            //Act

            var nextNews = sut.GetNextFiveOrTheRest(count);

            //Arrange

            Assert.AreEqual(nextNews.Count,5);
        }



        [TestMethod]
        public void GetNumberOfElements_WhenCalled_ExpectedToGetNumberOfNews()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            var allNews = sut.GetNumberOfElements();

            //Assert
            Assert.AreEqual(allNews,5);
        }

        [TestMethod]
        public void NewsExists_GivenAnId_ShouldReturnTrueIfNewsWithThatIdExists()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            var exists = sut.NewsExists(new Guid("e7c51c3a-3f56-40a7-832c-96246fdfe986"));

            //Assert

            Assert.AreEqual(exists,true);
        }


    }
}
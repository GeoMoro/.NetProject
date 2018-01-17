using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicesProvider;

namespace ServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        public UserRepositoryMock Mock = new UserRepositoryMock();

        [TestMethod]
        public void GetUsersByFactionId_WhenCalled_ExpectToGetUser()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetUsersByFactionId(Mock.UserStatuses[0].FactionId);

            // Assert
            Assert.AreEqual(result[0], Mock.UserStatuses[0]);
        }

        [TestMethod]
        public void CreateAndReturnLatestUser_WhenCalled_ExpectToGetUser()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.CreateAndReturnLatestUser(new Guid().ToString(), new Guid());

            // Assert
            Assert.AreEqual(Mock.UserStatuses.Count, 3);
        }

        [TestMethod]
        public void EditFaction_WhenCalled_ExpectToGetUser()
        {
            // Arrange
            var sut = CreateSut();
            var newId = new Guid();

            // Act
            sut.EditFaction(Mock.UserStatuses[0].Id, newId);

            // Assert
            Assert.AreEqual(Mock.UserStatuses[0].Id, newId.ToString());
        }

        private UserStatusService CreateSut()
        {
            return new UserStatusService(Mock);
        }

        #region Repository Mock Class

        public class UserRepositoryMock : IUserStatusRepository
        {
            public List<UserStatus> UserStatuses;

            public UserRepositoryMock()
            {
                UserStatuses = new List<UserStatus>
                {
                    UserStatus.CreateUsersStatus(new Guid().ToString(), new Guid()),
                    UserStatus.CreateUsersStatus(new Guid().ToString(), new Guid())
                };
            }

            public IReadOnlyList<UserStatus> GetUsersByFactionId(Guid id)
            {
                return UserStatuses.Where(user => user.FactionId == id).ToList();
            }

            public IReadOnlyList<UserStatus> GetAllUsers()
            {
                return UserStatuses.ToList();
            }

            public UserStatus GetUserById(string id)
            {
                return UserStatuses.FirstOrDefault(user => user.Id == id);
            }

            public void CreateUser(UserStatus user)
            {
                UserStatuses.Add(user);
            }

            public void EditUser(UserStatus user)
            {
                var userToBeEdited = GetUserById(user.Id);
            }

            public void DeleteUser(UserStatus user)
            {
                UserStatuses.Remove(user);
            }
        }

        #endregion
    }
}


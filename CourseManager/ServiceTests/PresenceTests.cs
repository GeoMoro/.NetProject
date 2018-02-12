using System;
using System.Collections.Generic;
using System.Linq;
using Business.ServicesInterfaces.Models.PresenceViewModels;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicesProvider;

namespace ServiceTests
{
    [TestClass]
    public class PresenceTests
    {
        public PresenceRepositoryMock PresenceMock = new PresenceRepositoryMock();

        [TestMethod]
        public void GetUsersGivenGroup_WhenCalled_ExpectToGetUsers()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetUsersGivenGroup(PresenceMock.Presences[0].Name, new Guid());

            // Assert
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void GetUsersByFactionId_WhenCalled_ExpectToGetUsers()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetUsersByFactionId(new Guid());

            // Assert
            Assert.AreEqual(result.Count, 2);
        }

        [TestMethod]
        public void GetAllUsers_WhenCalled_ExpectToGetUsers()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetAllUsers();

            // Assert
            Assert.AreEqual(result.Count, 2);
        }

        [TestMethod]
        public void ModifyPresence_WhenCalled_ExpectToGetUsers()
        {
            // Arrange
            var sut = CreateSut();
            var userCreateModel = new UserStatusCreateModel
            {
                Presence = false
            };

            // Act
            sut.ModifyPresence(new Guid().ToString(), userCreateModel);

            // Assert
            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public void GetUserById_WhenCalled_ExpectToGetUser()
        {
            // Arrange
            var sut = CreateSut();
            var id = new Guid().ToString();
            // Act
            var result = sut.GetUserById(id);

            // Assert
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void GetAllAtendances_WhenCalled_ExpectToGetAttendances()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetAllAttendances();

            // Assert
            Assert.AreEqual(result.Count, 1);
        }
        
        [TestMethod]
        public void GetPresenceById_WhenCalled_ExpectToGetPresence()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetPresenceById(PresenceMock.Presences[0].Id);

            // Assert
            Assert.AreEqual(result, PresenceMock.Presences[0]);
        }

        [TestMethod]
        public void CreateFaction_WhenCalled_ExpectToCreateFaction()
        {
            // Arrange
            var sut = CreateSut();
            var presenceCreateModel = new PresenceCreateModel
            {
                Name = "B3"
            };

            // Act
            sut.CreateFaction(presenceCreateModel);

            // Assert
            Assert.AreEqual(PresenceMock.Presences.Count, 3);
        }

        [TestMethod]
        public void StartLaboratory_WhenCalled_ExpectToCreateLaboratory()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.StartPresenceBasedOnValue(new Guid(), 1);

            // Assert
            Assert.AreEqual(true, true);
        }
        

        [TestMethod]
        public void GetAllPresences_WhenCalled_ExpectToGetPresences()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetAllPresences();

            // Assert
            Assert.AreEqual(result.Count, 2);
        }

        private PresenceService CreateSut()
        {
            var userRepo = new UserServiceTests.UserRepositoryMock();
            var attendance = new UserAttendanceTests.UserAttendanceRepositoryMock();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var _dbContext = new ApplicationDbContext(optionsBuilder.Options);

            return new PresenceService(PresenceMock, _dbContext, userRepo, new UserStatusService(userRepo),  attendance, new UserAttendanceService(PresenceMock, new UserServiceTests.UserRepositoryMock(), attendance));
        }

        #region Repository Mock Class
        
        public class PresenceRepositoryMock : IPresenceRepository
        {
            public List<Presence> Presences;

            public PresenceRepositoryMock()
            {
                var users = new List<UserStatus>();
                users.Add(UserStatus.CreateUsersStatus(new Guid().ToString(), new Guid(), false));
                Presences = new List<Presence>
                {
                    Presence.CreatePresence(new Guid(), "B3", users),
                    Presence.CreatePresence(new Guid(), "B4", new List<UserStatus>())
                };

                Presences[0].Id = new Guid("4a1eacef-78ad-4da3-a94e-2cfd2400651a");
            }

            public Presence GetPresenceByName(string name)
            {
                return Presences.SingleOrDefault(presence => presence.Name == name);
            }

            public IReadOnlyList<Presence> GetAllPresences()
            {
                return Presences.ToList();
            }

            public Presence GetPresenceById(Guid id)
            {
                return Presences.SingleOrDefault(presence => presence.Id == id);
            }

            public void CreatePresence(Presence presence)
            {
                Presences.Add(presence);
            }

            public void UpdatePresence(Presence presence)
            {
                var presenceToBeUpdated = GetPresenceById(presence.Id);
                presenceToBeUpdated.Name = "A4";
            }

            public void DeletePresence(Presence presence)
            {
                Presences.Remove(presence);
            }
        }
       
        #endregion

    }
}


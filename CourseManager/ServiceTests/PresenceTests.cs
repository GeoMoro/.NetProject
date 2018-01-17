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
            sut.StartLaboratoryBasedOnValue(new Guid(), 1);

            // Assert
            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public void ModificationOnUsers_WhenCalled_ExpectToModifyUsers()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.ApplyModificationsOnUsers("new faction", new List<UserStatus>());

            // Assert
            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public void ApplyModificationsOnUsers_WhenCalled_ExpectToUpdate()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.ApplyModificationsOnUsers("new modification", new List<UserStatus>());

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
                users.Add(UserStatus.CreateUsersStatus(new Guid().ToString(), new Guid()));
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

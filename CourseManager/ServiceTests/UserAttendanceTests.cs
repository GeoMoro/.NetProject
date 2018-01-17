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
    public class UserAttendanceTests
    {
        public UserAttendanceRepositoryMock Mock = new UserAttendanceRepositoryMock();

        [TestMethod]
        public void GetAttendanceByUserId_WhenCalled_ExpectToGetAttendance()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetAttendanceByUserId(Mock.Attendances[0].UserId.ToString());

            // Assert
            Assert.AreEqual(result[0], Mock.Attendances[0]);
        }

        [TestMethod]
        public void DeleteData_WhenCalled_ExpectToGetAttendance()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.DeleteData(Mock.Attendances[0].Id);

            // Assert
            Assert.AreEqual(Mock.Attendances.Count, 1);
        }
        
        private UserAttendanceService CreateSut()
        {
            return new UserAttendanceService(new PresenceTests.PresenceRepositoryMock() , new UserServiceTests.UserRepositoryMock(),  Mock);
        }

        #region Repository Mock Class

        public class UserAttendanceRepositoryMock : IAttendanceRepository
        {
            public List<Attendance> Attendances;

            public UserAttendanceRepositoryMock()
            {
                Attendances = new List<Attendance>
                {
                    Attendance.CreateAttendance(1, new Guid(), new Guid().ToString(), 10, 10, false)
                };
            }

            public IReadOnlyList<Attendance> GetAllAttendances()
            {
                return Attendances.ToList();
            }

            public Attendance GetAttendanceById(Guid id)
            {
                return Attendances.SingleOrDefault(attendance => attendance.Id == id);
            }

            public void CreateAttendance(Attendance attendance)
            {
                Attendances.Add(attendance);
            }

            public void EditAttendance(Attendance attendance)
            {
                var edit = GetAttendanceById(attendance.Id);
                edit.KataMark = 9;
                edit.LaboratoryMark = 9;
            }

            public void DeleteAttendance(Attendance attendance)
            {
                Attendances.Remove(attendance);
            }
        }

        #endregion
    }
}

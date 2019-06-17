using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Tests.PayCloudUserServicesTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.PayCloudUserServicesTests
{
    [TestClass]
    public class GetAllPayCloudUsersAsync_Should : PayCloudUserServicesMocks
    {
        [TestMethod]
        public async Task ReturnsAllPayCloudUsers()
        {
            //Arrange
            var ReturnsAllPayCloudUsers = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(ReturnsAllPayCloudUsers);
            PayCloudUserServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                sut = new PayCloudUserServices(
                    assertContext,
                    accountServiceMock.Object,
                    clientServiceMock.Object,
                    hashingServiceMock.Object,
                    DateTimeNowMock.Object,
                    LoggerMock.Object);

                for (int i = 2; i < 9; i++)
                {
                    await assertContext.PayCloudUsers.AddAsync(new PayCloudUser()
                    {
                        Name = $"PayCloudUser{i}",
                        Password = "a1234567",
                        Role = "User",
                        Username = $"UserName{i}",
                        UserId = i
                    });
                    await assertContext.SaveChangesAsync();
                }

                //Act
                var returnedUser = await sut.GetAllPayCloudUsersAsync();

                //Assert
                Assert.IsNotNull(returnedUser);
                Assert.AreEqual(7, returnedUser.Count);
            }
        }

        [TestMethod]
        public async Task ReturnsEmptyList()
        {
            //Arrange
            var ReturnsEmptyList = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(ReturnsEmptyList);
            PayCloudUserServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                sut = new PayCloudUserServices(
                    assertContext,
                    accountServiceMock.Object,
                    clientServiceMock.Object,
                    hashingServiceMock.Object,
                    DateTimeNowMock.Object,
                    LoggerMock.Object);

                //Act
                var returnedUsers = await sut.GetAllPayCloudUsersAsync();

                //Assert
                Assert.IsNotNull(returnedUsers);
                Assert.AreEqual(0, returnedUsers.Count);
            }
        }
    }
}

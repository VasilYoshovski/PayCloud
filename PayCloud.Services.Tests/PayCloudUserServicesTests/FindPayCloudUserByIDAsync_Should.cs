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
    public class FindPayCloudUserByIDAsync_Should : PayCloudUserServicesMocks
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task FindsBannerWhenValidIdIsPassed(int id)
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);
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

                await assertContext.PayCloudUsers.AddAsync(new PayCloudUser()
                {
                    Name = "PayCloudUser",
                    Password = "a1234567",
                    Role = "User",
                    Username = "UserName",
                    UserId = id
                });
                await assertContext.SaveChangesAsync();

                //Act
                var returnedUser = await sut.FindPayCloudUserByIDAsync(id);

                //Assert
                Assert.IsNotNull(returnedUser);
                Assert.AreEqual(id, returnedUser.UserId);
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(200)]
        public async Task ReturnsNullWhenInvalidIdIsPassed(int id)
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);
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

                await assertContext.PayCloudUsers.AddAsync(new PayCloudUser()
                {
                    Name = "PayCloudUser",
                    Password = "a1234567",
                    Role = "User",
                    Username = "UserName",
                    UserId = 2
                });
                await assertContext.SaveChangesAsync();

                //Act
                var returnedUser = await sut.FindPayCloudUserByIDAsync(id);

                //Assert
                Assert.IsNull(returnedUser);
            }
        }
    }
}

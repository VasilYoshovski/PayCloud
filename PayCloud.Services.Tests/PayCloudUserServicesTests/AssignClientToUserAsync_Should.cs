using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.PayCloudUserServicesTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.PayCloudUserServicesTests
{
    [TestClass]
    public class AssignClientToUserAsync_Should : PayCloudUserServicesMocks
    {
        [TestMethod]
        public async Task AssignsClientToUser()
        {
            //Arrange
            var AssignsAccountToUser = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(AssignsAccountToUser);

            var result = Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3Users: true);

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

                var userIdTemp = 1;
                var clientIdTemp = 1;

                //Act
                var returnedResult = await sut.AssignClientToUserAsync(clientIdTemp, userIdTemp);

                //Assert
                Assert.IsTrue(returnedResult);
            }
        }

        [TestMethod]
        public async Task ThrowsWhenClientAlreadyAssignedToUser()
        {
            //Arrange
            var ThrowsWhenClientAlreadyAssignedToUser = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(ThrowsWhenClientAlreadyAssignedToUser);

            var result = Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3Users: true);

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

                var userIdTemp = 1;
                var clientIdTemp = 1;

                //Act & Assert
                var returnedResult1 = await sut.AssignClientToUserAsync(clientIdTemp, userIdTemp);
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.AssignClientToUserAsync(clientIdTemp, userIdTemp));
            }
        }

        [TestMethod]
        public async Task ThrowsWhenClientIdInvalid()
        {
            //Arrange
            var ThrowsWhenClientIdInvalid = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(ThrowsWhenClientIdInvalid);

            var result = Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3Users: true);

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

                var userIdTemp = 1;
                var clientIdTemp = -1;

                //Act & Assert
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.AssignClientToUserAsync(clientIdTemp, userIdTemp));
            }
        }

        [TestMethod]
        public async Task ThrowsWhenUserIdInvalid()
        {
            //Arrange
            var ThrowsWhenUserIdInvalid = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(ThrowsWhenUserIdInvalid);

            var result = Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3Users: true);

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

                var userIdTemp = -1;
                var clientIdTemp = 1;

                //Act & Assert
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.AssignClientToUserAsync(clientIdTemp, userIdTemp));
            }
        }

        [TestMethod]
        public async Task ThrowsWhenClientIdAndUserIdAreInvalid()
        {
            //Arrange
            var ThrowsWhenClientIdAndUserIdAreInvalid = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(ThrowsWhenClientIdAndUserIdAreInvalid);

            var result = Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3Users: true);

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

                var userIdTemp = -1;
                var clientIdTemp = -1;

                //Act & Assert
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.AssignClientToUserAsync(clientIdTemp, userIdTemp));
            }
        }
    }
}

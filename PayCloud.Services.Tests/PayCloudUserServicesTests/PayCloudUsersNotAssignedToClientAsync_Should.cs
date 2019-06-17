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
    public class PayCloudUsersNotAssignedToClientAsync_Should : PayCloudUserServicesMocks
    {
        [TestMethod]
        public async Task GetsThreeUsersNotAssignedToClientWithId1()
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

                int? clientIdTemp = 1;

                //Act
                var returnedResult = await sut.PayCloudUsersNotAssignedToClientAsync(clientIdTemp);

                //Assert
                Assert.AreEqual(3, returnedResult.Count);
            }
        }

        [TestMethod]
        public async Task GetsThreeUsersNotAssignedToClientWithIdNull()
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

                int? clientIdTemp = null;

                //Act
                var returnedResult = await sut.PayCloudUsersNotAssignedToClientAsync(clientIdTemp);

                //Assert
                Assert.AreEqual(3, returnedResult.Count);
            }
        }

        [TestMethod]
        public async Task GetsTwoUsersNotAssignedToClientWithId1()
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

                int? clientIdTemp = 1;
                int? userIdTemp = 1;
                var result1 = await sut.AssignClientToUserAsync(clientIdTemp.Value, userIdTemp.Value);

                //Act
                var returnedResult = await sut.PayCloudUsersNotAssignedToClientAsync(clientIdTemp);

                //Assert
                Assert.IsTrue(result1);
                Assert.AreEqual(2, returnedResult.Count);
            }
        }
    }
}

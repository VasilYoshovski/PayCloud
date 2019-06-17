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
    public class AssignAcountToUserAsync_Should : PayCloudUserServicesMocks
    {
        [TestMethod]
        public async Task AssignsAccountToUser()
        {
            //Arrange
            var AssignsAccountToUser = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(AssignsAccountToUser);

            var result = Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3Users: true, seed3UserClients: true);

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
                var accountIdTemp = 3;

                //Act
                var returnedResult = await sut.AssignAcountToUserAsync(clientIdTemp, accountIdTemp, userIdTemp);

                //Assert
                Assert.IsTrue(returnedResult);
            }
        }
    }
}

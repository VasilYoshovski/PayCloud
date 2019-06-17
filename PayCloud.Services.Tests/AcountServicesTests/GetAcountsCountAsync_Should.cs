using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Dto;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class GetAcountsCountAsync_Should : AccountServiceMocks
    {
        [DataRow("Client2",2)]
        [DataRow("100",4)]
        [DataRow("003",1)]
        [TestMethod]
        public async Task ReturnProperCount_WhenValidContainsIsPassed(string contains, int expectedCount)
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts:true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAcountsCountAsync(contains);
                //Assert
                Assert.IsTrue(actual == expectedCount);
            }
        }

        [TestMethod]
        public async Task ReturnProperCount_WhenValidClientIdPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validClientId = 2;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAcountsCountAsync(clientId: validClientId);
                //Assert
                Assert.IsTrue(actual == 2);
            }
        }

        [TestMethod]
        public async Task ReturnProperCount_WhenBalanceTrueIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var haveBalance = true;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAcountsCountAsync(haveBalance: haveBalance);
                //Assert
                Assert.IsTrue(actual == 6);
            }
        }

        [TestMethod]
        public async Task ReturnProperCount_WhenAllParametersArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var haveBalance = true;
                var clientId = 2;
                var contains = "100";

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAcountsCountAsync(contains,clientId, haveBalance);
                //Assert
                Assert.IsTrue(actual == 2);
            }
        }


        [TestMethod]
        public async Task ReturnZero_WhenNonExistingIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var invalidClient = 100;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAcountsCountAsync(clientId: invalidClient);
                //Assert
                Assert.IsTrue(actual == 0);
            }
        }

    }
}

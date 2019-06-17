using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class GetTransactionsCountAsync_Should : TransactionServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperCount_WhenValidSkipAndTakeAndValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed8Transactions: true, seed3UserAccounts: true, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validUserId = 1;

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetTransactionsCountAsync(validUserId);
                //Assert
                Assert.IsTrue(actual == 16);
            }
        }

        [TestMethod]
        public async Task ReturnProperCount_WhenAccountIdAndValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed8Transactions: true, seed3UserAccounts: true, seed3Clients:true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validAccountId = 1;
                var validUserId = 1;


                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetTransactionsCountAsync(validUserId, validAccountId);
                var t = 0;
                //Assert
                Assert.IsTrue(actual == 7);
            }
        }


        [TestMethod]
        public async Task ReturnProperCount_WhenContainsAndValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed8Transactions: true, seed3UserAccounts: true, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validContains = "0001";
                var validUserId = 1;


                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetTransactionsCountAsync(validUserId, contains: validContains);

                //Assert
                Assert.IsTrue(actual == 14);
            }
        }

        [TestMethod]
        public async Task ReturnEmptyCollection_WhenInvalidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed8Transactions: true, seed3UserAccounts: true, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var skip = 1;
                var take = 3;
                var invalidUserId = 100;


                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetTransactionsCountAsync(invalidUserId);

                //Assert
                Assert.IsTrue(actual == 0);
            }
        }

    }
}

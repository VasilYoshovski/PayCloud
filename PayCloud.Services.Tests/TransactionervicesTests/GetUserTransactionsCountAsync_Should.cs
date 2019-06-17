using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class GetUserTransactionsCountAsync : TransactionServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperCount_WhenValidSkipAndTakeAndValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed8Transactions: true, seed3SavedTrans: true, seed3UserAccounts: true, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validUserId = 1;

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetUserTransactionsCountAsync(validUserId);
                var t = 0;
                //Assert
                Assert.IsTrue(actual == 10);
            }
        }

        [TestMethod]
        public async Task ReturnProperCount_WhenAccountIdAndValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed8Transactions: true, seed3SavedTrans: true, seed3UserAccounts: true, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validAccountId = 2;
                var validUserId = 1;


                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetUserTransactionsCountAsync(validUserId, validAccountId);
                var t = 0;
                //Assert
                Assert.IsTrue(actual == 2);
            }
        }


        [TestMethod]
        public async Task ReturnProperCount_WhenContainsAndValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed8Transactions: true, seed3SavedTrans: true, seed3UserAccounts: true, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validContains = "0001";
                var validUserId = 1;


                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetUserTransactionsCountAsync(validUserId, contains: validContains);
                var t = 0;

                //Assert
                Assert.IsTrue(actual == 8);
            }
        }

        [TestMethod]
        public async Task ReturnEmptyCollection_WhenInvalidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed8Transactions: true, seed3SavedTrans: true, seed3UserAccounts: true, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var invalidUserId = 100;


                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetUserTransactionsCountAsync(invalidUserId);

                //Assert
                Assert.IsTrue(actual == 0);
            }
        }

    }
}

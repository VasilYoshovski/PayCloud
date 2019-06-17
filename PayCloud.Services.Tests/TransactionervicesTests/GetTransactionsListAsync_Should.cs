using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class GetTransactionsListAsync_Should : TransactionServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperCollection_WhenValidSkipAndTakeAndValidUserIdArePassed()
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
                var validUserId = 1;
                var sortOrder = "TransactionId";

                var expectedTrabsactionIds = new List<int> {1,2,2 };
                var expectedTrabsactionAmounts = new List<decimal> {-10m,20m,-20m };

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetTransactionsListAsync(validUserId,skip: skip, take: take, sortOrder: sortOrder);
                //Assert
                Assert.IsTrue(actual.Count == 3);
                CollectionAssert.AreEquivalent(expectedTrabsactionIds, actual.Select(x => x.TransactionId).ToList());
                CollectionAssert.AreEquivalent(expectedTrabsactionAmounts, actual.Select(x => x.Amount).ToList());
            }
        }

        [TestMethod]
        public async Task ReturnProperCollection_WhenAccountIdAndValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed8Transactions: true, seed3UserAccounts: true, seed3Clients:true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var skip = 1;
                var take = 3;
                var sortOrder = "TransactionId";
                var validAccountId = 1;
                var validUserId = 1;

                var expectedTrabsactionIds = new List<int> { 2, 3, 4 };
                var expectedTrabsactionAmounts = new List<decimal> { 20m, 30m, -40m };


                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetTransactionsListAsync(validUserId, validAccountId, skip, take, sortOrder: sortOrder);
                var t = 0;
                //Assert
                Assert.IsTrue(actual.Count == 3);
                CollectionAssert.AreEquivalent(expectedTrabsactionIds, actual.Select(x => x.TransactionId).ToList());
                CollectionAssert.AreEquivalent(expectedTrabsactionAmounts, actual.Select(x => x.Amount).ToList());
            }
        }


        [TestMethod]
        public async Task ReturnProperCollection_WhenContainsAndValidUserIdArePassed()
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
                var sortOrder = "TransactionId";
                var validContains= "0001";
                var validUserId = 1;

                var expectedTrabsactionIds = new List<int> { 1, 2, 2 };
                var expectedTrabsactionAmounts = new List<decimal> { -10m, 20m, -20m };


                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                var actual = await sut.GetTransactionsListAsync(validUserId, contains:validContains, skip: skip, take: take, sortOrder: sortOrder);
                var t = 0;
                //Assert
                Assert.IsTrue(actual.Count == 3);
                CollectionAssert.AreEquivalent(expectedTrabsactionIds, actual.Select(x => x.TransactionId).ToList());
                CollectionAssert.AreEquivalent(expectedTrabsactionAmounts, actual.Select(x => x.Amount).ToList());
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
                var actual = await sut.GetTransactionsListAsync(invalidUserId, skip: skip, take: take);
                var t = 0;
                //Assert
                Assert.IsTrue(actual.Count == 0);
            }
        }
        
    }
}

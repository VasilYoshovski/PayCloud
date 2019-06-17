using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Dto;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class AccountsIdsAssignedToPayCloudUserAsync_Should : AccountServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperList_WhenValidUserIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3UserAccounts: true, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validUserId = 1;
                var collectionExpected = new List<int>() { 1, 2, 3 };

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.AccountsIdsAssignedToPayCloudUserAsync(validUserId);

                //Assert
                CollectionAssert.AreEqual(collectionExpected, actual);
                Assert.IsTrue(actual.Count == 3);
            }
        }

        [TestMethod]
        public async Task ReturnEmptyList_WhenInvalidUserIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validUserId = 1;
                var collectionExpected = new List<int>() { 1, 2 };

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.AccountsIdsAssignedToPayCloudUserAsync(validUserId);

                //Assert
                Assert.IsTrue(actual.Count == 0);
            }
        }

    }
}

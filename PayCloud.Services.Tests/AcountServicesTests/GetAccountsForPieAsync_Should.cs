using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using System.Collections.Generic;
using System.Linq;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class GetAccountsForPieAsync_Should : AccountServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperCollection_WhenValidUserIdPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var expectedNicknames = new List<string> { "TestUserAccounts11", "TestUserAccounts12", "TestUserAccounts13" };
                var expectedBalances = new List<decimal> { 100, 200, 300 };
                var validUserId = 1;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAccountsForPieAsync(validUserId);
                //Assert

                Assert.IsTrue(actual.Count == 3);
                CollectionAssert.AreEquivalent(expectedNicknames, actual.Select(x => x.AccountNickname).ToList());
                CollectionAssert.AreEquivalent(expectedBalances, actual.Select(x => x.Balance).ToList());
            }
        }

        [TestMethod]
        public async Task ReturnEmptyCollection_WhenInalidUserIdPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var invalidUserId = 100;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAccountsForPieAsync(invalidUserId);
                //Assert

                Assert.IsTrue(actual.Count == 0);
            }
        }
    }
}

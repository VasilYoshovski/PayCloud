using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class GetAllUserAccountsAsync_Should : AccountServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperCollection_WhenValidSkipAndTakeAndValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var skip = 1;
                var take = 1;
                var validUserId = 1;
                var sortOrder = "AccountId";

                var expectedAccount = assertContext.Accounts.Find(2);

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAllUserAccountsAsync(validUserId, skip, take, sortOrder: sortOrder);
                //Assert
                Assert.IsTrue(actual.Count == 1);
                Assert.AreEqual(expectedAccount.AccountId, actual.Single().AccountId);
            }
        }

        [TestMethod]
        public async Task ReturnProperCollection_WhenClientIdAndValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var sortOrder = "AccountId";
                var validClientId = 1;
                var validUserId = 1;


                var expectedAccount1 = assertContext.Accounts.Find(2);
                var expectedAccount2 = assertContext.Accounts.Find(3);

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAllUserAccountsAsync(validUserId, clientId: validClientId, sortOrder: sortOrder);
                //Assert
                Assert.IsTrue(actual.Count == 2);
                Assert.AreEqual(expectedAccount1.AccountId, actual.First().AccountId);
                Assert.AreEqual(expectedAccount2.AccountId, actual.Last().AccountId);
            }
        }



        [TestMethod]
        public async Task ReturnProperCollection_WhenOnlyValidUserIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var sortOrder = "AccountId";
                var validUserId = 1;
                var expectedAccounts = assertContext.UsersAccounts.Where(x => x.PayCloudUserId == validUserId).Select(x => x.Account).ToList();


                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAllUserAccountsAsync(userId: validUserId, sortOrder: sortOrder);
                var t = 0;

                //Assert
                Assert.IsTrue(actual.Count == 3);
                Assert.AreEqual(expectedAccounts[0].AccountId, actual.First().AccountId);
                Assert.AreEqual(expectedAccounts[1].AccountId, actual.Skip(1).First().AccountId);
                Assert.AreEqual(expectedAccounts[2].AccountId, actual.Last().AccountId);

            }
        }

        [TestMethod]
        public async Task ReturnProperCollection_WhenValidUserIdAndContainsArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var sortOrder = "AccountId";
                var contains = "Client3";
                var validUserId = 1;
                var expectedAccount = assertContext.Accounts.Find(1);


                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAllUserAccountsAsync(userId: validUserId, contains: contains, sortOrder: sortOrder);
                var t = 0;

                //Assert
                Assert.IsTrue(actual.Count == 1);
                Assert.AreEqual(expectedAccount.AccountId, actual.Single().AccountId);
            }
        }
    }
}

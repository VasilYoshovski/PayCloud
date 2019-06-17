using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class GetUserAcountsCountAsync_Should : AccountServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperCount_WhenValidUserIdAndHaveBalanceTrueArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var haveBalance = true;
                var validUserId = 1;

                var expectedAccount = assertContext.Accounts.Find(2);

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetUserAcountsCountAsync(validUserId, haveBalance: haveBalance);
                //Assert
                Assert.IsTrue(actual == 3);
            }
        }

        [TestMethod]
        public async Task ReturnProperCount_WhenClientIdAndValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validClientId = 1;
                var validUserId = 1;


                var expectedAccount1 = assertContext.Accounts.Find(2);
                var expectedAccount2 = assertContext.Accounts.Find(3);

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetUserAcountsCountAsync(validUserId, clientId: validClientId);
                //Assert
                Assert.IsTrue(actual == 2);
            }
        }



        [TestMethod]
        public async Task ReturnProperCount_WhenOnlyValidUserIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validUserId = 1;
                var expectedAccounts = assertContext.UsersAccounts.Where(x => x.PayCloudUserId == validUserId).Select(x => x.Account).ToList();


                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetUserAcountsCountAsync(userId: validUserId);
                var t = 0;

                //Assert
                Assert.IsTrue(actual == 3);

            }
        }

        [TestMethod]
        public async Task ReturnProperCount_WhenValidUserIdAndContainsArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var contains = "Client3";
                var validUserId = 1;
                var expectedAccount = assertContext.Accounts.Find(1);


                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetUserAcountsCountAsync(userId: validUserId, contains: contains);
                var t = 0;

                //Assert
                Assert.IsTrue(actual == 1);
            }
        }
    }
}

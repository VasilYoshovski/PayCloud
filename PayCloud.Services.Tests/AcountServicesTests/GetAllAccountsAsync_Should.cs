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
    public class GetAllAccountsAsync_Should : AccountServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperCollection_WhenValidSkipAndTakeArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var skip = 2;
                var take = 3;
                var sortOrder = "AccountId";

                var expectedAccount1 = assertContext.Accounts.Find(3);
                var expectedAccount2 = assertContext.Accounts.Find(4);
                var expectedAccount3 = assertContext.Accounts.Find(5);

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAllAccountsAsync(skip,take,sortOrder: sortOrder);
                //Assert
                Assert.IsTrue(actual.Count == 3);
                AccountMapperMock.Verify(x => x.MapFrom(expectedAccount1), Times.Once);
                AccountMapperMock.Verify(x => x.MapFrom(expectedAccount2), Times.Once);
                AccountMapperMock.Verify(x => x.MapFrom(expectedAccount3), Times.Once);
            }
        }

        [TestMethod]
        public async Task ReturnProperCollection_WhenValidSkipAndTakeAndClientIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var skip = 2;
                var take = 3;
                var sortOrder = "AccountId";
                var validClientId = 1;

                var expectedAccount = assertContext.Accounts.Find(6);

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAllAccountsAsync(skip, take,clientId:validClientId, sortOrder: sortOrder);
                //Assert
                Assert.IsTrue(actual.Count == 1);
                AccountMapperMock.Verify(x => x.MapFrom(expectedAccount), Times.Once);
            }
        }


        [TestMethod]
        public async Task ReturnProperCollection_WhenValidSkipAndTakeAndContainsArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var skip = 2;
                var take = 2;
                var sortOrder = "AccountId";
                var contains = "100";

                var expectedAccount1 = assertContext.Accounts.Find(5);
                var expectedAccount2 = assertContext.Accounts.Find(6);

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAllAccountsAsync(skip, take, contains: contains, sortOrder: sortOrder);
               
                //Assert
                Assert.IsTrue(actual.Count == 2);
                AccountMapperMock.Verify(x => x.MapFrom(expectedAccount1), Times.Once);
                AccountMapperMock.Verify(x => x.MapFrom(expectedAccount2), Times.Once);
            }
        }

        [TestMethod]
        public async Task ReturnProperCollection_WhenValidUserIdArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true, seed3MoreAccounts: true, seed3UserAccounts:true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var sortOrder = "AccountId";
                var validUserId = 1;
                var expectedAccounts = assertContext.UsersAccounts.Where(x => x.PayCloudUserId == validUserId).Select(x => x.Account).ToList();


                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAllAccountsAsync(userId: validUserId, sortOrder: sortOrder);
                var t = 0;

                //Assert
                Assert.IsTrue(actual.Count == 3);
                AccountMapperMock.Verify(x => x.MapFrom(expectedAccounts[0]), Times.Once);
                AccountMapperMock.Verify(x => x.MapFrom(expectedAccounts[1]), Times.Once);
                AccountMapperMock.Verify(x => x.MapFrom(expectedAccounts[2]), Times.Once);
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
                var actual = await sut.GetAllAccountsAsync(userId: validUserId, contains:contains, sortOrder: sortOrder);
                var t = 0;

                //Assert
                Assert.IsTrue(actual.Count == 1);
                AccountMapperMock.Verify(x => x.MapFrom(expectedAccount), Times.Once);
            }
        }
    }
}

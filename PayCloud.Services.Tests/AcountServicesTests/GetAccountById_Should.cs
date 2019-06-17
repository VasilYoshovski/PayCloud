using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using PayCloud.Services.Dto;
using Moq;
using PayCloud.Data.Models;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class GetAccountById_Should : AccountServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperType_WhenValidParameterIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validAccountId = 1;
                var account = assertContext.Accounts.Find(validAccountId);
                var accountDto = new AccountDto();

                AccountMapperMock.Setup(x => x.MapFrom(account)).Returns(accountDto);

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAccountByIdAsync(validAccountId);
                //Assert
                Assert.IsInstanceOfType(actual, typeof(AccountDto));
                AccountMapperMock.Verify(x => x.MapFrom(account), Times.Once);

            }
        }

        [TestMethod]
        public async Task ReturnProperDto_WhenValidParameterIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts:true, seed3Clients:true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validAccountId = 1;
                var account = assertContext.Accounts.Find(validAccountId);

                var accountDto = new AccountDto();
                AccountMapperMock.Setup(x => x.MapFrom(account)).Returns(accountDto);

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAccountByIdAsync(validAccountId);

                //Assert
                AccountMapperMock.Verify(x => x.MapFrom(account), Times.Once);
                Assert.AreEqual(accountDto, actual);
            }
        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidUserIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            using (var assertContext = new PayCloudDbContext(Seeder.GetOptions(databaseName)))
            {
                InitMocks();
                var invalidAccountId = 100;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.GetAccountByIdAsync(invalidAccountId));
            }

        }

    }
}

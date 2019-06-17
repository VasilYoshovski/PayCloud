using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class CreateAccountAsync_Should : AccountServiceMocks
    {
        [TestMethod]
        public async Task AddAccountToDataBase_WhenValidParametersArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validClientId = 1;
                var validBalance = 1000m;
                var accountDto = new AccountDto();

                RandomGeneratorMock.Setup(x => x.GenerateNumber()).Returns("123456789");
                ClientAccountMapperMock.Setup(x => x.MapFrom(It.IsAny<Account>())).Returns(new ClientAccountDto());

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.CreateAccountAsync(validBalance, validClientId);
                //Assert
                Assert.IsInstanceOfType(actual, typeof(ClientAccountDto));
                ClientAccountMapperMock.Verify(x => x.MapFrom(It.IsAny<Account>()), Times.Once);
                RandomGeneratorMock.Verify(x => x.GenerateNumber(), Times.Once);
                Assert.IsTrue(assertContext.Accounts.Count() == 1);
                Assert.IsTrue(assertContext.Accounts.Any(x => x.ClientId == validClientId && x.Balance == validBalance));


            }
        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidClientIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var invalidClientId = 100;
                var validBalance = 1000m;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.CreateAccountAsync(validBalance, invalidClientId))).Message,
                    Constants.ClientNotFound
                );
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenZeroOrLessClientIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var invalidClientId = 0;
                var validBalance = 100m;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.CreateAccountAsync(validBalance, invalidClientId))).Message,
                    Constants.WrongArguments
                );
            }

        }


        [TestMethod]
        public async Task ThrowServiceErrorException_WhenZeroOrLessBalanceIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validClientId = 1;
                var invalidBalance = 0m;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.CreateAccountAsync(invalidBalance, validClientId))).Message,
                Constants.WrongArguments
                );
            }

        }
    }
}

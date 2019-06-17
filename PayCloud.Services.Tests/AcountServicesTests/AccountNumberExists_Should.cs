using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using PayCloud.Services.Dto;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class AccountNumberExists_Should : AccountServiceMocks
    {
        [TestMethod]
        public async Task ReturnFalse_WhenValidNonExistingAccNumberIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var nonExistingAccountNumber = "0000000000";

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.AccountNumberExists(nonExistingAccountNumber);
                //Assert
                Assert.IsFalse(actual);
            }
        }

        [TestMethod]
        public async Task ReturnTrue_WhenValidExistingAccNumberIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts:true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var existingAccountNumber = "0000000001";

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.AccountNumberExists(existingAccountNumber);
                //Assert
                Assert.IsTrue(actual);
            }
        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenNullAccNumberIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            using (var assertContext = new PayCloudDbContext(Seeder.GetOptions(databaseName)))
            {
                InitMocks();
                string nullccountNumber = null;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.AccountNumberExists(nullccountNumber));
            }

        }

    }
}

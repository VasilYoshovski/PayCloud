using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class GetAccountNicknameAsync_Should: AccountServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperNickname_WhenValidParametersArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options,seed3Accounts:true, seed3Users:true, seed3UserAccounts:true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var expectedNickname = "TestUserAccounts11";
                var validUserId = 1;
                var validAccountId = 1;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.GetAccountNicknameAsync(validAccountId, validUserId);
                //Assert

                Assert.AreEqual(expectedNickname, actual);
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
                var expectedNickname = "TestUserAccounts11";
                var invalidUserId = 100;
                var validAccountId = 1;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.GetAccountNicknameAsync(validAccountId, invalidUserId));
            }
           
        }


        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidAccountIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            using (var assertContext = new PayCloudDbContext(Seeder.GetOptions(databaseName)))
            {
                InitMocks();
                var expectedNickname = "TestUserAccounts11";
                var validUserId = 1;
                var invalidAccountId = 100;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.GetAccountNicknameAsync(invalidAccountId, validUserId));
            }

        }
    }
}

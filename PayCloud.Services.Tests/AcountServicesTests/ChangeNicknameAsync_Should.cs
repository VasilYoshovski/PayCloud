using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Dto;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class ChangeNicknameAsync_Should : AccountServiceMocks
    {
        [TestMethod]
        public async Task ReturnOne_WhenValidParametersArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options,seed3Accounts:true,seed3Users:true,seed3UserAccounts:true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validNickName = "NewValidNickname";
                var validAccontId = 1;
                var validUserId = 2;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.ChangeNicknameAsync(validAccontId, validUserId, validNickName);
                //Assert
                Assert.IsTrue(actual == 1);
            }
        }

        [TestMethod]
        public async Task ChangeEntityNickname_WhenValidParametersArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validNickname = "NewValidNickname";
                var validAccontId = 1;
                var validUserId = 2;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.ChangeNicknameAsync(validAccontId, validUserId, validNickname);
                //Assert
                Assert.AreEqual(validNickname, assertContext.UsersAccounts.Find(validUserId, validAccontId).AccountNickname);
            }
        }


        [TestMethod]
        public async Task ThrowServiceErrorException_WhenZeroOrLessAccountIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            using (var assertContext = new PayCloudDbContext(Seeder.GetOptions(databaseName)))
            {
                var validNickname = "NewValidNickname";
                var invalidAccontId = 0;
                var validUserId = 2;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.ChangeNicknameAsync(invalidAccontId, validUserId, validNickname))).Message,
                    Constants.WrongArguments
                );
            }
        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenZeroOrLessUserIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            using (var assertContext = new PayCloudDbContext(Seeder.GetOptions(databaseName)))
            {
                var validNickname = "NewValidNickname";
                var validAccontId = 1;
                var invalidUserId = 0;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.ChangeNicknameAsync(validAccontId, invalidUserId, validNickname))).Message,
                    Constants.WrongArguments
                );
            }
        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenNullNicknameIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            using (var assertContext = new PayCloudDbContext(Seeder.GetOptions(databaseName)))
            {
                string invalidNickname = null;
                var validAccontId = 1;
                var validUserId = 2;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.ChangeNicknameAsync(validAccontId, validUserId, invalidNickname))).Message,
                    Constants.WrongArguments
                );
            }
        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenNotExistingUserIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                var validNickname = "NewValidNickname";
                var validAccontId = 1;
                var invalidUserId = 100;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.ChangeNicknameAsync(validAccontId, invalidUserId, validNickname))).Message,
                    Constants.AccountNotExist
                );
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenNotExistingAccountIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed3UserAccounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                var validNickname = "NewValidNickname";
                var invalidAccontId = 1;
                var validUserId = 100;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.ChangeNicknameAsync(invalidAccontId, validUserId, validNickname))).Message,
                    Constants.AccountNotExist
                );
            }
        }

        }
    }

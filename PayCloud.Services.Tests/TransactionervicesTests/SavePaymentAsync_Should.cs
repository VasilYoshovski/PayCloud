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


namespace PayCloud.Services.Tests.TransactionervicesTests
{
    [TestClass]
    public class SavePaymentAsync_Should : TransactionServiceMocks
    {
        [TestMethod]
        public async Task AddTransactionToDB_WhenValidDtoIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3UserAccounts: true, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(2)).ReturnsAsync(true);

                var transactionCUDto = new TransactionCUDto
                {
                    SenderAccountId = 2,
                    ReceiverAccountId = 1,
                    Amount = 5,
                    CreatedByUserId = 2,
                    Description = "Test payment",
                    StatusCode = 0
                };

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                await sut.SavePaymentAsync(transactionCUDto);
                //Assert
                DateTimeProviderMock.Verify(x => x.Now, Times.Once);
                Assert.IsTrue(assertContext.Transactions.Count() == 1);
                Assert.IsTrue(assertContext.Transactions.Any
                    (x =>
                        x.ReceiverAccountId == transactionCUDto.ReceiverAccountId
                        && x.SenderAccountId == transactionCUDto.SenderAccountId
                        && x.Amount == transactionCUDto.Amount
                        && x.CreatedByUserId == transactionCUDto.CreatedByUserId
                        && x.StatusCode == transactionCUDto.StatusCode
                        && x.Description == transactionCUDto.Description
                    ));
            }
        }

        [TestMethod]
        public async Task ThrowAccountAccessSuspendedException_WhenUserNotAuthorized()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                    InitMocks();
                    var transactionCUDto = new TransactionCUDto()
                    {
                        SenderAccountId = 2,
                        ReceiverAccountId = 1,
                        Amount = 5,
                        CreatedByUserId = 2,
                        Description = "Test payment",
                        StatusCode = 0
                    };
                    
                    var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                    //Act & Assert
                    Assert.AreEqual(
                    (await Assert.ThrowsExceptionAsync<AccountAccessSuspendedException>(() => sut.SavePaymentAsync(transactionCUDto))).Message,
                    string.Format(Constants.AccountAccessSuspended, transactionCUDto.SenderAccountId));
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenSenderAndReceiverAccountsAreSame()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3UserAccounts: true, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(1)).ReturnsAsync(true);

                var validUserId = 1;
                var transactionCUDto = new TransactionCUDto()
                {
                    SenderAccountId = 1,
                    ReceiverAccountId = 1,
                    Amount = 5,
                    CreatedByUserId = 2,
                    Description = "Test payment",
                    StatusCode = 0
                };

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act & Assert
                Assert.AreEqual(
                (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.SavePaymentAsync(transactionCUDto))).Message,
                Constants.SameAccounts);
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidSenderAccountIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3UserAccounts: true, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(100)).ReturnsAsync(true);

                var transactionCUDto = new TransactionCUDto()
                {
                    SenderAccountId = 100,
                    ReceiverAccountId = 3,
                    Amount = 5,
                    CreatedByUserId = 1,
                    Description = "Test payment",
                    StatusCode = 0
                };

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act & Assert
                Assert.AreEqual(
                (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.SavePaymentAsync(transactionCUDto))).Message,
                string.Format(Constants.AccountWithIdNotExist, transactionCUDto.SenderAccountId));
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidReceiverAccountIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts:true ,seed3UserAccounts: true, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(1)).ReturnsAsync(true);

                var transactionCUDto = new TransactionCUDto()
                {
                    SenderAccountId = 1,
                    ReceiverAccountId = 100,
                    Amount = 5,
                    CreatedByUserId = 1,
                    Description = "Test payment",
                    StatusCode = (int)StatusCode.Saved
                };

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act & Assert
                Assert.AreEqual(
                (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.SavePaymentAsync(transactionCUDto))).Message,
                string.Format(Constants.AccountWithIdNotExist, transactionCUDto.ReceiverAccountId));
            }

        }
    }
}

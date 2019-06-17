using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PayCloud.Services.Tests.TransactionervicesTests
{
    [TestClass]
    public class CancelPaymentAsync_Should : TransactionServiceMocks
    {
        [TestMethod]
        public async Task ChangeTransactionStatusCode_WhenValidIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3UserAccounts: true,
                seed3Users: true, seed8Transactions: true, seed3SavedTrans: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(1)).ReturnsAsync(true);

                var validTransactionId = 9;
                var senderAccountBalanceBeforTransaction = assertContext.Accounts.Find(1).Balance;
                var receiverAccountBalanceBeforTransaction = assertContext.Accounts.Find(2).Balance;
                var transactionAmount = assertContext.Transactions.Find(validTransactionId).Amount;
                var expectedSenderAccountBalance = senderAccountBalanceBeforTransaction - transactionAmount;
                var expectedReceiverAccountBalance = receiverAccountBalanceBeforTransaction + transactionAmount;

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                await sut.CancelPaymentAsync(validTransactionId);
                //Assert
                Assert.IsTrue(assertContext.Transactions.Find(validTransactionId).StatusCode == (int)StatusCode.Canceled);

            }
        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidTransactionIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed8Transactions: true, seed3SavedTrans: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                var invalidTransactionId = 100;

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act & Assert
                Assert.AreEqual(
                string.Format(Constants.TransactionNotExist),
                (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.CancelPaymentAsync(invalidTransactionId))).Message
                );
            }

        }


        [TestMethod]
        public async Task ThrowAccountAccessSuspendedException_WhenUserNotAuthorized()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed8Transactions: true, seed3SavedTrans: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                var validTransactionId = 9;

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act & Assert
                Assert.AreEqual(
                string.Format(Constants.AccountAccessSuspended, "1"),
                (await Assert.ThrowsExceptionAsync<AccountAccessSuspendedException>(() => sut.CancelPaymentAsync(validTransactionId))).Message
                );
            }

        }


    }
}

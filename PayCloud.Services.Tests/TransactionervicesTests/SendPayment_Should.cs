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
    public class SendPayment_Should : TransactionServiceMocks
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

                DateTimeProviderMock.Setup(x => x.Now).Returns(new DateTime(2019, 06, 10));
                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(1)).ReturnsAsync(true);

                var validTransactionId = 9;
                var senderAccountBalanceBeforTransaction = assertContext.Accounts.Find(1).Balance;
                var receiverAccountBalanceBeforTransaction = assertContext.Accounts.Find(2).Balance;
                var transactionAmount = assertContext.Transactions.Find(validTransactionId).Amount;
                var expectedSenderAccountBalance = senderAccountBalanceBeforTransaction - transactionAmount;
                var expectedReceiverAccountBalance = receiverAccountBalanceBeforTransaction + transactionAmount;

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                await sut.SendPayment(validTransactionId);
                //Assert
                Assert.IsTrue(assertContext.Transactions.Find(validTransactionId).StatusCode == (int)StatusCode.Sent);

            }
        }

        [TestMethod]
        public async Task SetTransactionSentOn_WhenValidIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3UserAccounts: true,
                seed3Users: true, seed8Transactions: true, seed3SavedTrans: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                DateTimeProviderMock.Setup(x => x.Now).Returns(new DateTime(2019, 06, 10));
                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(1)).ReturnsAsync(true);

                var validTransactionId = 9;
                var senderAccountBalanceBeforTransaction = assertContext.Accounts.Find(1).Balance;
                var receiverAccountBalanceBeforTransaction = assertContext.Accounts.Find(2).Balance;
                var transactionAmount = assertContext.Transactions.Find(validTransactionId).Amount;
                var expectedSenderAccountBalance = senderAccountBalanceBeforTransaction - transactionAmount;
                var expectedReceiverAccountBalance = receiverAccountBalanceBeforTransaction + transactionAmount;

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                await sut.SendPayment(validTransactionId);
                //Assert
                Assert.IsTrue(assertContext.Transactions.Find(validTransactionId).SentOn == new DateTime(2019, 06, 10));

            }
        }

        [TestMethod]
        public async Task DateTimeNowProviderIsExecutedOnce_WhenValidIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3UserAccounts: true,
                seed3Users: true, seed8Transactions: true, seed3SavedTrans: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                DateTimeProviderMock.Setup(x => x.Now).Returns(new DateTime(2019, 06, 10));
                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(1)).ReturnsAsync(true);

                var validTransactionId = 9;
                var senderAccountBalanceBeforTransaction = assertContext.Accounts.Find(1).Balance;
                var receiverAccountBalanceBeforTransaction = assertContext.Accounts.Find(2).Balance;
                var transactionAmount = assertContext.Transactions.Find(validTransactionId).Amount;
                var expectedSenderAccountBalance = senderAccountBalanceBeforTransaction - transactionAmount;
                var expectedReceiverAccountBalance = receiverAccountBalanceBeforTransaction + transactionAmount;

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                await sut.SendPayment(validTransactionId);
                //Assert
                DateTimeProviderMock.Verify(x => x.Now, Times.Exactly(1));

            }
        }

        [TestMethod]
        public async Task ChangeSenderAndReceiverAccountBalances_WhenValidIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3UserAccounts: true,
                seed3Users: true, seed8Transactions: true, seed3SavedTrans: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                DateTimeProviderMock.Setup(x => x.Now).Returns(new DateTime(2019, 06, 10));
                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(1)).ReturnsAsync(true);

                var validTransactionId = 9;
                var senderAccountBalanceBeforTransaction = assertContext.Accounts.Find(1).Balance;
                var receiverAccountBalanceBeforTransaction = assertContext.Accounts.Find(2).Balance;
                var transactionAmount = assertContext.Transactions.Find(validTransactionId).Amount;
                var expectedSenderAccountBalance = senderAccountBalanceBeforTransaction - transactionAmount;
                var expectedReceiverAccountBalance = receiverAccountBalanceBeforTransaction + transactionAmount;

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act
                await sut.SendPayment(validTransactionId);
                //Assert
                Assert.IsTrue(assertContext.Accounts.Find(1).Balance == expectedSenderAccountBalance);
                Assert.IsTrue(assertContext.Accounts.Find(2).Balance == expectedReceiverAccountBalance);

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
                string.Format(Constants.AccountNotExist),
                (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.SendPayment(invalidTransactionId))).Message
                );
            }

        }


        [TestMethod]
        public async Task ThrowAccountAccessSuspendedException_WhenUserNotAuthorized()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed8Transactions:true, seed3SavedTrans: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                var validTransactionId = 9;

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act & Assert
                Assert.AreEqual(
                string.Format(Constants.AccountAccessSuspended, "1"),
                (await Assert.ThrowsExceptionAsync<AccountAccessSuspendedException>(() => sut.SendPayment(validTransactionId))).Message
                );
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenSenderAndReceiverAccountsAreSame()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true);
            using (var seedContext = new PayCloudDbContext(options))
            {
                var transaction = new Transaction()
                {
                    TransactionId = 1,
                    Amount = 10,
                    SenderAccountId = 1,
                    ReceiverAccountId = 1,
                    StatusCode = (int)StatusCode.Saved,
                    Description = "Test saved transaction",
                    CreatedOn = new DateTime(2019, 6, 10),
                    CreatedByUserId = 1
                };
                seedContext.Transactions.Add(transaction);
                seedContext.SaveChanges();
            }


            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(1)).ReturnsAsync(true);

                var validTransactionId = 1;


                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act & Assert
                Assert.AreEqual(
                Constants.SameAccounts,
                (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.SendPayment(validTransactionId))).Message
                );
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidSenderAccountIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true);
            using (var seedContext = new PayCloudDbContext(options))
            {
                var transaction = new Transaction()
                {
                    TransactionId = 1,
                    Amount = 10,
                    SenderAccountId = 100,
                    ReceiverAccountId = 1,
                    StatusCode = (int)StatusCode.Saved,
                    Description = "Test saved transaction",
                    CreatedOn = new DateTime(2019, 6, 10),
                    CreatedByUserId = 1
                };
                seedContext.Transactions.Add(transaction);
                seedContext.SaveChanges();
            }
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
                    StatusCode = (int)StatusCode.Sent
                };

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act & Assert
                Assert.AreEqual(
                string.Format(Constants.AccountWithIdNotExist, transactionCUDto.SenderAccountId),
                (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.SendPayment(1))).Message
                );
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidReceiverAccountIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true);
            using (var seedContext = new PayCloudDbContext(options))
            {
                var transaction = new Transaction()
                {
                    TransactionId = 1,
                    Amount = 10,
                    SenderAccountId = 1,
                    ReceiverAccountId = 100,
                    StatusCode = (int)StatusCode.Saved,
                    Description = "Test saved transaction",
                    CreatedOn = new DateTime(2019, 6, 10),
                    CreatedByUserId = 1
                };
                seedContext.Transactions.Add(transaction);
                seedContext.SaveChanges();
            }
            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(1)).ReturnsAsync(true);

                var validTransactionId = 9;


                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act & Assert
                Assert.AreEqual(
                string.Format(Constants.AccountWithIdNotExist, ""),
                (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.SendPayment(1))).Message
                );
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenTransactionAmountIsBiggerThanBalanceIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true);
            using (var seedContext = new PayCloudDbContext(options))
            {
                var transaction = new Transaction()
                {
                    TransactionId = 1,
                    Amount = 1000,
                    SenderAccountId = 1,
                    ReceiverAccountId = 2,
                    StatusCode = (int)StatusCode.Saved,
                    Description = "Test saved transaction",
                    CreatedOn = new DateTime(2019, 6, 10),
                    CreatedByUserId = 1
                };
                seedContext.Transactions.Add(transaction);
                seedContext.SaveChanges();
            }
            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                UserServiceMock.Setup(x => x.IsUserAuthorizedForAccount(1)).ReturnsAsync(true);

                var sut = new TransactionServices(assertContext, UserServiceMock.Object, DateTimeProviderMock.Object);

                //Act & Assert
                Assert.AreEqual(
                string.Format(Constants.InsufficientBalance, assertContext.Accounts.Find(1).AccountNumber),
                (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.SendPayment(1))).Message
                );
            }

        }
    }
}

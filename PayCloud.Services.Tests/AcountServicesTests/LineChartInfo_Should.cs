using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PayCloud.Services.Tests.AcountServicesTests
{
    [TestClass]
    public class LineChartInfo_Should : AccountServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperCollection_WhenValidUserIdPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed8Transactions: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                DateTimeNowMock.Setup(x => x.Now).Returns(new DateTime(2019, 6, 11));

                var expectedDates = new List<DateTime>
                {
                    new DateTime(2019,6,7), new DateTime(2019, 6, 8), new DateTime(2019, 6, 10), new DateTime(2019, 6, 11)
                };

                var expectedBalances = new List<decimal> { 140, 180, 90, 100 };
                var validAccountId = 1;
                var validDays = 7;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.LineChartInfo(validAccountId, validDays);
                //Assert

                Assert.IsTrue(actual.Count == 4);
                CollectionAssert.AreEquivalent(expectedDates, actual.Select(x => x.Date).ToList());
                CollectionAssert.AreEquivalent(expectedBalances, actual.Select(x => x.Balance).ToList());
            }
        }

        [TestMethod]
        public async Task ReturnEmptyCollection_WhenInvalidUserIdPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed8Transactions: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                DateTimeNowMock.Setup(x => x.Now).Returns(new DateTime(2019, 6, 11));

                var invalidAccountId = 100;
                var validDays = 7;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.LineChartInfo(invalidAccountId, validDays);
                //Assert

                Assert.IsTrue(actual.Count == 0);
            }
        }

        [TestMethod]
        public async Task ReturnEmptyCollection_WhenInvalidDaysIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Accounts: true, seed3Users: true, seed8Transactions: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                DateTimeNowMock.Setup(x => x.Now).Returns(new DateTime(2019, 6, 11));

                var validAccountId = 100;
                var invalidDays = -1;

                var sut = new AccountService(assertContext, RandomGeneratorMock.Object, DateTimeNowMock.Object, ClientAccountMapperMock.Object, AccountMapperMock.Object);

                //Act
                var actual = await sut.LineChartInfo(validAccountId, invalidDays);
                //Assert

                Assert.IsTrue(actual.Count == 0);
            }
        }

    }
}

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Services.Providers;
using PayCloud.Services.Tests.BannerServicesTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.BannerServicesTests
{
    [TestClass]
    public class BannerServicesConstructor_Should : BannerServicesMock
    {
        [TestMethod]
        public void Constructor_CreatesInstance()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act
                sut = new BannerServices(
                    assertContext,
                    DateTimeNowMock.Object,
                    LoggerMock.Object);
            }

            //Assert
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void Constructor_ThrowsWhenContextNull()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);
            BannerServices sut;

            InitMocks();

            //Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new BannerServices(
                    null,
                    DateTimeNowMock.Object,
                    LoggerMock.Object));
        }

        [TestMethod]
        public void Constructor_ThrowsWhenDateTimeProviderNull()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new BannerServices(
                    assertContext,
                    null,
                    LoggerMock.Object));
            }
        }

        [TestMethod]
        public void Constructor_ThrowsWhenLoggerProviderNull()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new BannerServices(
                    assertContext,
                    DateTimeNowMock.Object,
                    null));
            }
        }
    }
}

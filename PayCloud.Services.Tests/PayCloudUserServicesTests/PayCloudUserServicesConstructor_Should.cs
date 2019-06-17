using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.BannerServicesTests.Utils;
using PayCloud.Services.Tests.PayCloudUserServicesTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Tests.PayCloudUserServicesTests
{
    [TestClass]
    public class PayCloudUserServicesConstructor_Should : PayCloudUserServicesMocks
    {
        [TestMethod]
        public void Constructor_CreatesInstance()
        {
            //Arrange
            var Constructor_CreatesInstance = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(Constructor_CreatesInstance);
            PayCloudUserServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act
                sut = new PayCloudUserServices(
                    assertContext,
                    accountServiceMock.Object,
                    clientServiceMock.Object,
                    hashingServiceMock.Object,
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

            InitMocks();

            //Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new PayCloudUserServices(
                    null,
                    accountServiceMock.Object,
                    clientServiceMock.Object,
                    hashingServiceMock.Object,
                    DateTimeNowMock.Object,
                    LoggerMock.Object));
        }

        [TestMethod]
        public void Constructor_ThrowsWhenAccountServiceProviderNull()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new PayCloudUserServices(
                    assertContext,
                    null,
                    clientServiceMock.Object,
                    hashingServiceMock.Object,
                    DateTimeNowMock.Object,
                    LoggerMock.Object));
            }
        }

        [TestMethod]
        public void Constructor_ThrowsWhenClientServiceProviderNull()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new PayCloudUserServices(
                    assertContext,
                    accountServiceMock.Object,
                    null,
                    hashingServiceMock.Object,
                    DateTimeNowMock.Object,
                    LoggerMock.Object));
            }
        }

        [TestMethod]
        public void Constructor_ThrowsWhenHashingProviderNull()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new PayCloudUserServices(
                    assertContext,
                    accountServiceMock.Object,
                    clientServiceMock.Object,
                    null,
                    DateTimeNowMock.Object,
                    LoggerMock.Object));
            }
        }

        [TestMethod]
        public void Constructor_ThrowsWhenDateTimeProviderNull()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new PayCloudUserServices(
                    assertContext,
                    accountServiceMock.Object,
                    clientServiceMock.Object,
                    hashingServiceMock.Object,
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

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new PayCloudUserServices(
                    assertContext,
                    accountServiceMock.Object,
                    clientServiceMock.Object,
                    hashingServiceMock.Object,
                    DateTimeNowMock.Object,
                    null));
            }
        }
    }
}

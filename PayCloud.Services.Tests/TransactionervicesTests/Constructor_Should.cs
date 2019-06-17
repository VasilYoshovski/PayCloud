using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Services.Providers;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Tests.BannerServicesTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.TransactionervicesTests
{
    [TestClass]
    public class Constructor_Should : TransactionServiceMocks
    {
        [TestMethod]
        public void Constructor_CreatesInstance()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act
                var sut = new TransactionServices(
                    assertContext,
                    UserServiceMock.Object,
                    DateTimeProviderMock.Object
          );


                //Assert
                Assert.IsNotNull(sut);
            }
        }

        [TestMethod]
        public void Constructor_ThrowsWhenContextNull()
        {
            //Arrange
            InitMocks();

            //Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new TransactionServices(
                null,
                UserServiceMock.Object,
                DateTimeProviderMock.Object));
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
                Assert.ThrowsException<ArgumentNullException>(() => new TransactionServices(
                    assertContext,
                    UserServiceMock.Object,
                    null));
            }
        }

        [TestMethod]
        public void Constructor_ThrowsWhenRandomGeneratorNull()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new TransactionServices(
                    assertContext,
                    null,
                    DateTimeProviderMock.Object));
            }
        }
       
    }
}

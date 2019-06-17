using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Identity;
using PayCloud.Services.Tests.IdentityTests.AdminSeviceTests.Utils;
using System;

namespace PayCloud.Services.Tests.IdentityTests.AdminSeviceTests
{
    [TestClass]
    public class Constructor_Should : AdminServiceMocks
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
                var sut = new AdminServices(
                    assertContext,
                    AuthorizationServiceMock.Object,
                    HashingServiceMock.Object);


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
            Assert.ThrowsException<ArgumentNullException>(() => new AdminServices(
                null,
                AuthorizationServiceMock.Object,
                HashingServiceMock.Object));
        }

        [TestMethod]
        public void Constructor_ThrowsWhenAuthorizationServiceNull()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                //Act & Assert

                //Act & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new AdminServices(
                    assertContext,
                    null,
                    HashingServiceMock.Object));
            }
        }

        [TestMethod]
        public void Constructor_ThrowsWhenHashingServiceNull()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();


                //Act & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new AdminServices(
                    assertContext,
                    AuthorizationServiceMock.Object,
                    null));
            }
        }
        
    }
}

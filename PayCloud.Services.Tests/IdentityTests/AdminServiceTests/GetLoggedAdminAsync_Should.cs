using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Identity;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Tests.IdentityTests.AdminSeviceTests.Utils;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PayCloud.Services.Tests.IdentityTests.AdminSeviceTests
{
    [TestClass]
    public class GetLoggedAdminAsync_Should : AdminServiceMocks
    {
        [TestMethod]
        public async Task GetProperLoggedAdmin_WhenLoggedUserIsValidAdmin()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed1Admin: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                AuthorizationServiceMock.Setup(x => x.GetLoggedUserId()).Returns(1);

                var sut = new AdminServices(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act
                var actual = await sut.GetLoggedAdminAsync();
                //Assert
                Assert.IsTrue(actual != null);
                Assert.IsTrue(actual.AdminId == 1);
                Assert.IsTrue(actual.Role == "Admin");
            }
        }

        [TestMethod]
        public async Task GetAdminWithoutPassword_WhenValidParametersArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed1Admin: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                AuthorizationServiceMock.Setup(x => x.GetLoggedUserId()).Returns(1);

                var sut = new AdminServices(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act
                var actual = await sut.GetLoggedAdminAsync();
                //Assert
                Assert.IsTrue(actual != null);
                Assert.IsTrue(actual.Password == "");
            }
        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidUsernameIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed1Admin: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                AuthorizationServiceMock.Setup(x => x.GetLoggedUserId()).Returns(2);

                var sut = new AdminServices(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    string.Format(Constants.UserNotFound, "2"),
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.GetLoggedAdminAsync())).Message
                );
            }

        }
    }
}
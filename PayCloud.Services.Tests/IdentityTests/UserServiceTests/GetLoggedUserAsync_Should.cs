using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Identity;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Tests.IdentityTests.UserSeviceTests.Utils;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PayCloud.Services.Tests.IdentityTests.UserSeviceTests
{
    [TestClass]
    public class GetLoggedUserAsync_Should : UserServiceMocks
    {
        [TestMethod]
        public async Task GetProperLoggedUser_WhenLoggedUserIsValidUser()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                AuthorizationServiceMock.Setup(x => x.GetLoggedUserId()).Returns(1);

                var sut = new UserService(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act
                var actual = await sut.GetLoggedUserAsync();
                //Assert
                Assert.IsTrue(actual != null);
                Assert.IsTrue(actual.UserId == 1);
                Assert.IsTrue(actual.Role == "User");
            }
        }

        [TestMethod]
        public async Task GetUserWithoutPassword_WhenValidParametersArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                AuthorizationServiceMock.Setup(x => x.GetLoggedUserId()).Returns(1);

                var sut = new UserService(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act
                var actual = await sut.GetLoggedUserAsync();
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

            Seeder.SeedDatabase(options, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                AuthorizationServiceMock.Setup(x => x.GetLoggedUserId()).Returns(4);

                var sut = new UserService(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    string.Format(Constants.UserNotFound, "4"),
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.GetLoggedUserAsync())).Message
                );
            }

        }
    }
}
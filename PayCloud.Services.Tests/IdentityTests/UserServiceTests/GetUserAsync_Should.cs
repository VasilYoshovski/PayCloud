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
    public class GetUserAsync_Should : UserServiceMocks
    {
        [TestMethod]
        public async Task GetProperUser_WhenValidParametersArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validUsername = "TestUserName1";
                var validPassword = "12345678";
                var validHashedPassword = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F";

                HashingServiceMock.Setup(x => x.GetHashedString(validPassword)).Returns(validHashedPassword);

                var sut = new UserService(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act
                var actual = await sut.GetUserAsync(validUsername, validPassword);
                //Assert
                Assert.IsTrue(actual != null);
                Assert.IsTrue(actual.UserId == 1);
                Assert.IsTrue(actual.Username == validUsername);
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
                var validUsername = "TestUserName1";
                var validPassword = "12345678";
                var validHashedPassword = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F";

                HashingServiceMock.Setup(x => x.GetHashedString(validPassword)).Returns(validHashedPassword);

                var sut = new UserService(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act
                var actual = await sut.GetUserAsync(validUsername, validPassword);
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
                var invalidUsername = "invaliduser";
                var validPassword = "12345678";
                var validHashedPassword = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F";

                HashingServiceMock.Setup(x => x.GetHashedString(validPassword)).Returns(validHashedPassword);

                var sut = new UserService(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    Constants.WrongCredentials,
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.GetUserAsync(invalidUsername, validPassword))).Message
                );
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidPasswordIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Users: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validUsername = "TestUserName1";
                var invalidPassword = "invalid";
                var validHashedPassword = "invalid psassword hash";

                HashingServiceMock.Setup(x => x.GetHashedString(invalidPassword)).Returns(validHashedPassword);

                var sut = new UserService(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    Constants.WrongCredentials,
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.GetUserAsync(validUsername, invalidPassword))).Message
                );
            }

        }
    }
}
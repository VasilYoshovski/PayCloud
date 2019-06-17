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
    public class GetAdminAsync_Should : AdminServiceMocks
    {
        [TestMethod]
        public async Task GetProperAdmin_WhenValidParametersArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed1Admin: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validUsername = "testadmin";
                var validPassword = "12345678";
                var validHashedPassword = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F";

                HashingServiceMock.Setup(x => x.GetHashedString(validPassword)).Returns(validHashedPassword);

                var sut = new AdminServices(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act
                var actual = await sut.GetAdminAsync(validUsername, validPassword);
                //Assert
                Assert.IsTrue(actual != null);
                Assert.IsTrue(actual.AdminId == 1);
                Assert.IsTrue(actual.Username == validUsername);
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
                var validUsername = "testadmin";
                var validPassword = "12345678";
                var validHashedPassword = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F";

                HashingServiceMock.Setup(x => x.GetHashedString(validPassword)).Returns(validHashedPassword);

                var sut = new AdminServices(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act
                var actual = await sut.GetAdminAsync(validUsername, validPassword);
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
                var invalidUsername = "invalidadmin";
                var validPassword = "12345678";
                var validHashedPassword = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F";

                HashingServiceMock.Setup(x => x.GetHashedString(validPassword)).Returns(validHashedPassword);

                var sut = new AdminServices(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    Constants.WrongCredentials,
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.GetAdminAsync(invalidUsername, validPassword))).Message
                );
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidPasswordIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed1Admin: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validUsername = "testadmin";
                var invalidPassword = "invalid";
                var validHashedPassword = "invalid psassword hash";

                HashingServiceMock.Setup(x => x.GetHashedString(invalidPassword)).Returns(validHashedPassword);

                var sut = new AdminServices(assertContext, AuthorizationServiceMock.Object, HashingServiceMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    Constants.WrongCredentials,
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.GetAdminAsync(validUsername, invalidPassword))).Message
                );
            }

        }
    }
}
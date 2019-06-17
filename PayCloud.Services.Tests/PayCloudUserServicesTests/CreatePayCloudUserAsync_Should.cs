using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Tests.PayCloudUserServicesTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.PayCloudUserServicesTests
{
    [TestClass]
    public class CreatePayCloudUserAsync_Should : PayCloudUserServicesMocks
    {
        [TestMethod]
        [DataRow("UserName", "NickName", "Password", "Role", 3)]
        public async Task CreatesUserWhenValidUserNameIsPassed(string userName, string name, string paswsord, string role, int id)
        {
            //Arrange
            var FindsBannerWhenValidUserNameIsPassed = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(FindsBannerWhenValidUserNameIsPassed);
            PayCloudUserServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                sut = new PayCloudUserServices(
                    assertContext,
                    accountServiceMock.Object,
                    clientServiceMock.Object,
                    hashingServiceMock.Object,
                    DateTimeNowMock.Object,
                    LoggerMock.Object);

                var createdUser = await sut.CreatePayCloudUserAsync(
                    name,
                    userName,
                    paswsord,
                    role);

                //Act
                var returnedUser = await sut.FindPayCloudUserByIDAsync(createdUser.UserId);

                //Assert
                Assert.IsNotNull(returnedUser);
                Assert.AreEqual(createdUser.UserId, returnedUser.UserId);
            }
        }

        [TestMethod]
        [DataRow("UName", "NickName", "Password", "Role", 3)]
        public async Task ReturnsNullWhenExistingUserNameIsPassed(string userName, string name, string paswsord, string role, int id)
        {
            //Arrange
            var ReturnsNullWhenExistingUserNameIsPassed = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(ReturnsNullWhenExistingUserNameIsPassed);
            PayCloudUserServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                sut = new PayCloudUserServices(
                    assertContext,
                    accountServiceMock.Object,
                    clientServiceMock.Object,
                    hashingServiceMock.Object,
                    DateTimeNowMock.Object,
                    LoggerMock.Object);


                var createdUser1 = await sut.CreatePayCloudUserAsync(
                    name,
                    userName,
                    paswsord,
                    role);

                //Act & Assert
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.CreatePayCloudUserAsync(
                    name,
                    userName,
                    paswsord,
                    role));
            }
        }
    }
}

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
    public class FindPayCloudUserByUserNameAsync_Should : PayCloudUserServicesMocks
    {
        [TestMethod]
        [DataRow("UName", "NickName", "Password", "Role", 3)]
        public async Task FindsBannerWhenValidUserNameIsPassed(string userName, string name, string paswsord, string role, int id)
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

                await assertContext.PayCloudUsers.AddAsync(new PayCloudUser()
                {
                    Name = name,
                    Password = paswsord,
                    Role = role,
                    Username = userName,
                    UserId = id
                });
                await assertContext.SaveChangesAsync();

                //Act
                var returnedUser = await sut.FindPayCloudUserByUserNameAsync(userName);

                //Assert
                Assert.IsNotNull(returnedUser);
                Assert.AreEqual(id, returnedUser.UserId);
            }
        }

        [TestMethod]
        [DataRow("UName", "NickName", "Password", "Role", 3)]
        public async Task ReturnsNullWhenInvalidUserNameIsPassed(string userName, string name, string paswsord, string role, int id)
        {
            //Arrange
            var ReturnsNullWhenInvalidUserNameIsPassed = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(ReturnsNullWhenInvalidUserNameIsPassed);
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

                await assertContext.PayCloudUsers.AddAsync(new PayCloudUser()
                {
                    Name = name,
                    Password = paswsord,
                    Role = role,
                    Username = "aaaaaaa",
                    UserId = id
                });
                await assertContext.SaveChangesAsync();

                //Act
                var returnedUser = await sut.FindPayCloudUserByUserNameAsync(userName);

                //Assert
                Assert.IsNull(returnedUser);
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Tests.BannerServicesTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.BannerServicesTests
{
    [TestClass]
    public class FindBannerByIDAsync_Should : BannerServicesMock
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task FindsBannerWhenValidIdIsPassed(int id)
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                sut = new BannerServices(
                    assertContext,
                    DateTimeNowMock.Object,
                    LoggerMock.Object);
                await assertContext.Banners.AddAsync(new Banner()
                {
                    UrlLink = "urlLink",
                    StartDate = DateTimeNowMock.Object.Now,
                    EndDate = DateTimeNowMock.Object.Now,
                    BannerId = id,
                    ImgLocationPath = "somePath"
                });
                await assertContext.SaveChangesAsync();

                //Act
                var returnedBanner = await sut.FindBannerByIDAsync(id);

                //Assert
                Assert.IsNotNull(returnedBanner);
                Assert.AreEqual(id, returnedBanner.BannerId);
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(200)]
        public async Task ReturnsNullWhenInvalidIdIsPassed(int id)
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                sut = new BannerServices(
                    assertContext,
                    DateTimeNowMock.Object,
                    LoggerMock.Object);

                await assertContext.Banners.AddAsync(new Banner()
                {
                    UrlLink = "urlLink",
                    StartDate = DateTimeNowMock.Object.Now,
                    EndDate = DateTimeNowMock.Object.Now,
                    BannerId = 1,
                    ImgLocationPath = "somePath"
                });
                await assertContext.SaveChangesAsync();

                //Act
                var returnedBanner = await sut.FindBannerByIDAsync(id);

                //Assert
                Assert.IsNull(returnedBanner);
            }
        }
    }
}

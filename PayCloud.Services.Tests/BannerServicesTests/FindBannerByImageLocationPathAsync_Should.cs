using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Tests.BannerServicesTests.Utils;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.BannerServicesTests
{
    [TestClass]
    public class FindBannerByImageLocationPathAsync_Should : BannerServicesMock
    {
        [TestMethod]
        [DataRow("Val", "Val")]
        [DataRow("ValidPath", "VaLiDPath")]
        public async Task FindsBannerWhenValidLocationIsPassed(string path, string searchPath)
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            RandomProvider rp = new RandomProvider();

            var options = Seeder.GetOptions(databaseName);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                sut = new BannerServices(
                    assertContext,
                    DateTimeNowMock.Object,
                    LoggerMock.Object,
                    rp);

                await assertContext.Banners.AddAsync(new Banner() {
                    UrlLink = "urlLink",
                    StartDate = DateTimeNowMock.Object.Now,
                    EndDate = DateTimeNowMock.Object.Now,
                    BannerId = 2,
                    ImgLocationPath = path
                });
                await assertContext.SaveChangesAsync();

                //Act
                var returnedBanner = await sut.FindBannerByImageLocationPathAsync(searchPath);

                //Assert
                Assert.IsNotNull(returnedBanner);
                Assert.AreEqual(path, returnedBanner.ImgLocationPath);
            }
        }

        [TestMethod]
        [DataRow("rtrtrt", "")]
        [DataRow("rtrtrt", null)]
        [DataRow("Val", "z")]
        [DataRow("Val", "Vali")]
        [DataRow("ValidPath", "kjjkjkjkjkjkj")]
        [DataRow("ValidPath", "xValidPath")]
        public async Task ReturnsNullWhenInvalidLocationIsPassed(string path, string searchPath)
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            RandomProvider rp = new RandomProvider();

            var options = Seeder.GetOptions(databaseName);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();

                sut = new BannerServices(
                    assertContext,
                    DateTimeNowMock.Object,
                    LoggerMock.Object,
                    rp);

                await assertContext.Banners.AddAsync(new Banner()
                {
                    UrlLink = "urlLink",
                    StartDate = DateTimeNowMock.Object.Now,
                    EndDate = DateTimeNowMock.Object.Now,
                    BannerId = 2,
                    ImgLocationPath = path
                });
                await assertContext.SaveChangesAsync();

                //Act
                var returnedBanner = await sut.FindBannerByImageLocationPathAsync(searchPath);

                //Assert
                Assert.IsNull(returnedBanner);
            }
        }
    }
}

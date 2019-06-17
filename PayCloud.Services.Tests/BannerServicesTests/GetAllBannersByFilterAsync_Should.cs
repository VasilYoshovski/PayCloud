using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.BannerServicesTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.BannerServicesTests
{
    [TestClass]
    public class GetAllBannersByFilterAsync_Should : BannerServicesMock
    {
        [TestMethod]
        public async Task ReturnsAllBasnners()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                DateTime startDate = DateTimeNowMock.Object.Now;
                DateTime endDate = DateTimeNowMock.Object.Now;

                sut = new BannerServices(
                    assertContext,
                    DateTimeNowMock.Object,
                    LoggerMock.Object);

                var returnedBanner1 = await sut.CreateBannerAsync("ImagaPath1", "UrlLink1", startDate, endDate);
                var returnedBanner2 = await sut.CreateBannerAsync("ImagaPath2", "UrlLink2", startDate, endDate);
                var returnedBanner3 = await sut.CreateBannerAsync("ImagaPath3", "UrlLink3", startDate, endDate);

                //Act
                var testResult = await sut.GetAllBannersByFilterAsync(0, 2, "");

                //Assert
                Assert.AreEqual(2, testResult.filteredList.Count);
                Assert.AreEqual(3, testResult.allCount);
                Assert.IsNotNull(returnedBanner1);
                Assert.IsNotNull(returnedBanner2);
                Assert.IsNotNull(returnedBanner3);
            }
        }

        [TestMethod]
        public async Task ReturnsEmptyList()
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

                //Act
                var testResult = await sut.GetAllBannersByFilterAsync(0, 3, "h");

                //Assert
                Assert.AreEqual(0, testResult.filteredList.Count);
                Assert.AreEqual(0, testResult.allCount);
            }
        }
    }
}

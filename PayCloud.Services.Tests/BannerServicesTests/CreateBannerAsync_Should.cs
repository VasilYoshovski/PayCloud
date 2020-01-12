using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.BannerServicesTests.Utils;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.BannerServicesTests
{
    [TestClass]
    public class CreateBannerAsync_Should : BannerServicesMock
    {
        [TestMethod]
        [DataRow("imagePath", "urlLink")]
        public async Task CreatesBannerWhenValidInputsArePassed(
            string imgPath,
            string urlLink)
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            RandomProvider rp = new RandomProvider();

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
                    LoggerMock.Object,
                    rp);

                //Act
                var returnedBanner = await sut.CreateBannerAsync(imgPath, urlLink, startDate, endDate);

                //Assert
                Assert.IsNotNull(returnedBanner);
            }
        }

        [TestMethod]
        [DataRow("imagePath1", "urlLink1")]
        public async Task ReturnsNullWhenBannerWithTheSameImagePathAlreadyExists(
            string imgPath,
            string urlLink)
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            RandomProvider rp = new RandomProvider();

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
                    LoggerMock.Object,
                    rp);

                //Act
                var returnedBanner1 = await sut.CreateBannerAsync(imgPath, urlLink, startDate, endDate);
                var returnedBanner2 = await sut.CreateBannerAsync(imgPath, urlLink, startDate, endDate);

                //Assert
                Assert.IsNotNull(returnedBanner1);
                Assert.IsNull(returnedBanner2);
            }
        }
    }
}

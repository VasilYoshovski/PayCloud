using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.BannerServicesTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.BannerServicesTests
{
    [TestClass]
    public class DeleteBannerAsync_Should : BannerServicesMock
    {
        [TestMethod]
        [DataRow("imagePath1", "urlLink1")]
        public async Task ReturnsTrueWhenBannerExistsAndIsDeleted(string imgPath, string urlLink)
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

                var returnedBanner = await sut.CreateBannerAsync(imgPath, urlLink, startDate, endDate);

                //Act
                var testResult = await sut.DeleteBannerAsync(returnedBanner.BannerId);

                //Assert
                Assert.IsTrue(testResult);
                Assert.IsNull(await sut.FindBannerByIDAsync(returnedBanner.BannerId));
            }
        }

        [TestMethod]
        [DataRow("imagePath1", "urlLink1")]
        public async Task ReturnsTrueWhenBannerDoesNotExist(
            string imgPath,
            string urlLink)
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
                var testResult = await sut.DeleteBannerAsync(5000);

                //Assert
                Assert.IsTrue(testResult);
                Assert.IsNull(await sut.FindBannerByIDAsync(5000));
            }
        }

        //int f()
        //{
        //    throw new DbUpdateException("hhhhh", new Exception("gggg"));
        //    return 5;
        //}

        //[TestMethod]
        //[DataRow("imagePath1", "urlLink1")]
        //public async Task ReturnsFalseWhenDatabaseErrorHappens(
        //    string imgPath,
        //    string urlLink)
        //{
        //    //Arrange
        //    var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    var options = Seeder.GetOptions(databaseName);
        //    BannerServices sut;

        //    using (var assertContext = new PayCloudDbContext(options))
        //    {
        //        InitMocks();
        //DateTime startDate = DateTimeNowMock.Object.Now;
        //DateTime endDate = DateTimeNowMock.Object.Now;

        //        sut = new BannerServices(
        //            assertContext,
        //            DateTimeNowMock.Object,
        //            LoggerMock.Object);

        //        var returnedBanner = await sut.CreateBannerAsync(imgPath, urlLink, startDate, endDate);

        //        var contextMock = new Mock<PayCloudDbContext> (options);
        //        contextMock.Setup(m => m.SaveChanges())
        //            .Returns(() => f());

        //        var sutFake = new BannerServices(
        //            contextMock.Object,
        //            DateTimeNowMock.Object,
        //            LoggerMock.Object);

        //        //Act & Assert
        //        await Assert.ThrowsExceptionAsync<DbUpdateException>(async () => await sutFake.DeleteBannerAsync(returnedBanner.BannerId));
        //    }
        //}
    }
}

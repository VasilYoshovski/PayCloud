using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Tests.BannerServicesTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.BannerServicesTests
{
    [TestClass]
    public class GetRandomSublistOfActiveBannersAsync_Should : BannerServicesMock
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(5)]
        [DataRow(30)]
        [DataRow(90)]
        public async Task ReturnsRandomBanners(int count)
        {
            //Arrange
            var ReturnsRandomBanners = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(ReturnsRandomBanners);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                DateTime startDate = DateTimeNowMock.Object.Now.AddDays(-1);
                DateTime endDate = DateTimeNowMock.Object.Now.AddDays(1);

                sut = new BannerServices(
                    assertContext,
                    DateTimeNowMock.Object,
                    LoggerMock.Object);

                for (int i = 0; i < 100; i++)
                {
                    await sut.CreateBannerAsync($"ImagaPath{i}", $"UrlLink{i}", startDate, endDate);
                }

                //Act & Assert
                var testResultOld = new List<Banner>();
                int sameCounter = 0;
                for (int i = 0; i < 100; i++)
                {
                    var hasDiferent = false;
                    var testResult = await sut.GetRandomSublistOfActiveBannersAsync(count);
                    Assert.IsNotNull(testResult);
                    Assert.AreEqual(count, testResult.Count);
                    var sharedCount = testResultOld.Intersect(testResult).Count();
                    if (testResultOld.Distinct().Count() > sharedCount || testResult.Distinct().Count() > sharedCount)
                    {
                        // there are different elements
                        hasDiferent = true;
                    }
                    {
                        // they contain the same elements
                    }
                    if (false == hasDiferent)
                    {
                        sameCounter++;
                    }
                    testResultOld = testResult;
                    Assert.IsTrue(sameCounter < 5);
                }
            }
        }

        [TestMethod]
        [DataRow(0)]
        public async Task ReturnsEmptyList(int count)
        {
            //Arrange
            var ReturnsEmptyList = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(ReturnsEmptyList);
            BannerServices sut;

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                DateTime startDate = DateTimeNowMock.Object.Now.AddDays(-1);
                DateTime endDate = DateTimeNowMock.Object.Now.AddDays(1);

                sut = new BannerServices(
                    assertContext,
                    DateTimeNowMock.Object,
                    LoggerMock.Object);

                for (int i = 0; i < 100; i++)
                {
                    await sut.CreateBannerAsync($"ImagaPath{i}", $"UrlLink{i}", startDate, endDate);
                }

                //Act & Assert
                var testResultOld = new List<Banner>();
                for (int i = 0; i < 100; i++)
                {
                    var testResult = await sut.GetRandomSublistOfActiveBannersAsync(count);
                    Assert.IsNotNull(testResult);
                    Assert.AreEqual(count, testResult.Count);
                }
            }
        }
    }
}

using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using PayCloud.Services.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Tests.BannerServicesTests.Utils
{
    public class BannerServicesMock
    {
        static BannerServicesMock()
        {
            InitMocks();
        }

        public static Mock<DateTimeNowProvider> DateTimeNowMock { get; set; }
        public static Mock<ILogger<BannerServices>> LoggerMock { get; set; }

        public static void InitMocks()
        {
            LoggerMock = new Mock<ILogger<BannerServices>>();
            DateTimeNowMock = new Mock<DateTimeNowProvider>();
        }
    }
}

using Moq;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Mappers;
using PayCloud.Services.Providers;

namespace PayCloud.Services.Tests.AcountServicesTests.Utils
{
    public class TransactionServiceMocks
    {
        static TransactionServiceMocks()
        {
            InitMocks();
        }

        public static Mock<IUserService> UserServiceMock { get; set; }
        public static Mock<IDateTimeNowProvider> DateTimeProviderMock { get; set; }


        public static void InitMocks()
        {
            UserServiceMock = new Mock<IUserService>();
            DateTimeProviderMock = new Mock<IDateTimeNowProvider>();
        }
    }
}
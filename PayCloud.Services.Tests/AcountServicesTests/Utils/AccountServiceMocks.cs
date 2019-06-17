using Moq;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Mappers;
using PayCloud.Services.Providers;
using PayCloud.Services.Utils;

namespace PayCloud.Services.Tests.AcountServicesTests.Utils
{
    public class AccountServiceMocks
    {
        static AccountServiceMocks()
        {
            InitMocks();
        }
        public static Mock<IRandom10Generator> RandomGeneratorMock { get; set; }
        public static Mock<IDateTimeNowProvider> DateTimeNowMock { get; set; }
        public static Mock<IDtoMapper<Account, ClientAccountDto>> ClientAccountMapperMock { get; set; }
        public static Mock<IDtoMapper<Account, AccountDto>> AccountMapperMock { get; set; }

        public static void InitMocks()
        {
            RandomGeneratorMock = new Mock<IRandom10Generator>();
            ClientAccountMapperMock = new Mock<IDtoMapper<Account, ClientAccountDto>>();
            AccountMapperMock = new Mock<IDtoMapper<Account, AccountDto>>();
            DateTimeNowMock =new Mock<IDateTimeNowProvider>();

    }
}
}
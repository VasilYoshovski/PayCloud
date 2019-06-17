using Microsoft.Extensions.Logging;
using Moq;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Mappers;
using PayCloud.Services.Providers;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Tests.PayCloudUserServicesTests.Utils
{
    public class PayCloudUserServicesMocks
    {
        static PayCloudUserServicesMocks()
        {
            InitMocks();
        }

        public static Mock<DateTimeNowProvider> DateTimeNowMock { get; set; }
        public static Mock<ILogger<PayCloudUserServices>> LoggerMock { get; set; }
        public static Mock<IAccountService> accountServiceMock { get; set; }
        public static Mock<IDtoMapper<Account, AccountDto>> AccountMapperMock { get; set; }
        public static Mock<IClientService> clientServiceMock { get; set; }
        public static Mock<IDtoMapper<Client, ClientDto>> ClientMapperMock { get; set; }
        public static Mock<IDtoMapper<Account, ClientAccountDto>> ClientAccountMapperMock { get; set; }
        public static Mock<IRandom10Generator> RandomGeneratorMock { get; set; }
        public static Mock<IHashingService> hashingServiceMock { get; set; }

        public static void InitMocks()
        {
            DateTimeNowMock = new Mock<DateTimeNowProvider>();
            LoggerMock = new Mock<ILogger<PayCloudUserServices>>();
            accountServiceMock = new Mock<IAccountService>();
            AccountMapperMock = new Mock<IDtoMapper<Account, AccountDto>>();
            clientServiceMock = new Mock<IClientService>();
            ClientMapperMock = new Mock<IDtoMapper<Client, ClientDto>>();
            ClientAccountMapperMock = new Mock<IDtoMapper<Account, ClientAccountDto>>();
            RandomGeneratorMock = new Mock<IRandom10Generator>();
            hashingServiceMock = new Mock<IHashingService>();
        }
    }
}

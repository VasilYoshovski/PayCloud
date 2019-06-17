using Moq;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Mappers;

namespace PayCloud.Services.Tests.AcountServicesTests.Utils
{
    public class ClientServiceMocks
    {
        static ClientServiceMocks()
        {
            InitMocks();
        }
        public static Mock<IDtoMapper<Client, ClientDto>> ClientMapperMock { get; set; }
        public static Mock<IDtoMapper<Account, ClientAccountDto>> ClientAccountMapperMock { get; set; }


        public static void InitMocks()
        {
            ClientMapperMock = new Mock<IDtoMapper<Client, ClientDto>>();
            ClientAccountMapperMock = new Mock<IDtoMapper<Account, ClientAccountDto>>();
        }
    }
}
using Moq;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Mappers;
using PayCloud.Services.Providers;

namespace PayCloud.Services.Tests.IdentityTests.AdminSeviceTests.Utils
{
    public class AdminServiceMocks
    {
        static AdminServiceMocks()
        {
            InitMocks();
        }

        public static Mock<IAuthorizationService> AuthorizationServiceMock { get; set; }
        public static Mock<IHashingService> HashingServiceMock { get; set; }


        public static void InitMocks()
        {
            AuthorizationServiceMock = new Mock<IAuthorizationService>();
            HashingServiceMock = new Mock<IHashingService>();
        }
    }
}
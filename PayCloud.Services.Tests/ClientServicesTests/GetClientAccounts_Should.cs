using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.ClientServicesTests
{
    [TestClass]
    public class GetClientAccounts_Should : ClientServiceMocks
    {

        [TestMethod]
        public async Task ReturnProperCount_WhenValidClientIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true, seed3Accounts: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validClientId = 1;

                var expectedAccounts = new List<Account> { assertContext.Accounts.Find(2), assertContext.Accounts.Find(3) };

                ClientAccountMapperMock.Setup(x => x.MapFrom(It.IsIn<Account>(expectedAccounts))).Returns(new ClientAccountDto());

                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act
                var actual = await sut.GetClientAccounts(validClientId);

                //Assert
                Assert.IsTrue(actual.Count == 2);
                ClientAccountMapperMock.Verify(x => x.MapFrom(It.IsIn<Account>(expectedAccounts)), Times.Exactly(2));
            }
        }


        [TestMethod]
        public async Task ReturnEmptyCollection_WhenInValidClientIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true, seed3MoreClients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var invalidClientId = 100;


                ClientAccountMapperMock.Setup(x => x.MapFrom(It.IsAny<Account>())).Returns(new ClientAccountDto());

                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act
                var actual = await sut.GetClientAccounts(invalidClientId);
                //Assert

                Assert.IsTrue(actual.Count == 0);
                ClientAccountMapperMock.Verify(x => x.MapFrom(It.IsAny<Account>()), Times.Never);
            }
        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenZeroOrLessClientIdIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);


            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var invalidClientId = 0;


                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    Constants.WrongArguments,
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.GetClientAccounts(invalidClientId))).Message
                );
            }

        }
    }
}

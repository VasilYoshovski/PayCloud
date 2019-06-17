using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using PayCloud.Services.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.ClientServicesTests
{
    [TestClass]
    public class CreateClientAsync_Should : ClientServiceMocks
    {
        [TestMethod]
        public async Task AddClientToDataBase_WhenValidClientNameIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var validClientName = "TestCLient";
                var accountDto = new AccountDto();

                ClientMapperMock.Setup(x => x.MapFrom(It.IsAny<Client>())).Returns(new ClientDto());

                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act
                var actual = await sut.CreateClientAsync(validClientName);
                //Assert
                Assert.IsInstanceOfType(actual, typeof(ClientDto));
                ClientMapperMock.Verify(x => x.MapFrom(It.IsAny<Client>()), Times.Once);
                Assert.IsTrue(assertContext.Clients.Count() == 1);
                Assert.IsTrue(assertContext.Clients.Any(x => x.Name == validClientName));


            }
        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenInvalidClientNameIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);


            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                string invalidClientName = null;

                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.CreateClientAsync(invalidClientName))).Message,
                    Constants.WrongArguments
                );
            }

        }

        [TestMethod]
        public async Task ThrowServiceErrorException_WhenExistingClientNameIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                string existsClientName = "TestClient1";

                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act & Assert
                Assert.AreEqual(
                    string.Format(Constants.ClientNameExist, existsClientName),
                    (await Assert.ThrowsExceptionAsync<ServiceErrorException>(() => sut.CreateClientAsync(existsClientName))).Message
                );
            }

        }

       
    }
}

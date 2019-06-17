using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.ClientServicesTests
{
    [TestClass]
    public class GetClientsListAsync_Should : ClientServiceMocks
    {
        [TestMethod]
        public async Task ReturnProperCollection_WhenValidSkipAndTakeArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var skip = 1;
                var take = 2;
                var sortOrder = "ClientId";

                var expectedClients = new List<Client> { assertContext.Clients.Find(2), assertContext.Clients.Find(3) };

                ClientMapperMock.Setup(x => x.MapFrom(It.IsIn<Client>(expectedClients))).Returns(new ClientDto());

                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act
                var actual = await sut.GetClientsListAsync(skip, take, sortOrder: sortOrder);
                //Assert
                Assert.IsTrue(actual.Count == 2);
                ClientMapperMock.Verify(x => x.MapFrom(It.IsIn<Client>(expectedClients)), Times.Exactly(2));

            }
        }

        [TestMethod]
        public async Task ReturnProperCollection_WhenValidSkipAndTakeAndTermArePassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true, seed3MoreClients:true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var skip = 1;
                var take = 2;
                var term = "ent3";
                var sortOrder = "ClientId";

                var expectedClients = new List<Client> { assertContext.Clients.Find(4), assertContext.Clients.Find(5) };

                ClientMapperMock.Setup(x => x.MapFrom(It.IsIn<Client>(expectedClients))).Returns(new ClientDto());

                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act
                var actual = await sut.GetClientsListAsync(skip, take, term, sortOrder);
                //Assert

                Assert.IsTrue(actual.Count == 2);
                ClientMapperMock.Verify(x => x.MapFrom(It.IsIn<Client>(expectedClients)), Times.Exactly(2));
            }
        }

        [TestMethod]
        public async Task ReturnProperCollection_WhenValidTermIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true, seed3MoreClients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var term = "ent33";
                var sortOrder = "ClientId";

                var expectedClients = new List<Client> { assertContext.Clients.Find(6) };

                ClientMapperMock.Setup(x => x.MapFrom(It.IsIn<Client>(expectedClients))).Returns(new ClientDto());

                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act
                var actual = await sut.GetClientsListAsync(term: term, sortOrder: sortOrder);
                //Assert

                Assert.IsTrue(actual.Count == 1);
                ClientMapperMock.Verify(x => x.MapFrom(It.IsIn<Client>(expectedClients)), Times.Once);
            }
        }

        [TestMethod]
        public async Task ReturnEmptyCollection_WhenInValidTermIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true, seed3MoreClients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var invalidTerm = "ent100";
                var sortOrder = "ClientId";


                ClientMapperMock.Setup(x => x.MapFrom(It.IsAny<Client>())).Returns(new ClientDto());

                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act
                var actual = await sut.GetClientsListAsync(term: invalidTerm, sortOrder: sortOrder);
                //Assert

                Assert.IsTrue(actual.Count == 0);
                ClientMapperMock.Verify(x => x.MapFrom(It.IsAny<Client>()), Times.Never);
            }
        }
    }
}

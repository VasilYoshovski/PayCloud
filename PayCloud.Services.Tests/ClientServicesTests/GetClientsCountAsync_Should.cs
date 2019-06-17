using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.DbContext;
using PayCloud.Services.Tests.AcountServicesTests.Utils;
using System.Threading.Tasks;

namespace PayCloud.Services.Tests.ClientServicesTests
{
    [TestClass]
    public class GetClientsCountAsync_Should : ClientServiceMocks
    {

        [TestMethod]
        public async Task ReturnProperCount_WhenValidTermIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true, seed3MoreClients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var term = "ent3";

                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act
                var actual = await sut.GetClientsCountAsync(term);
                
                //Assert
                Assert.IsTrue(actual == 4);
            }
        }

        

        [TestMethod]
        public async Task ReturnZeroCount_WhenInValidTermIsPassed()
        {
            //Arrange
            var databaseName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var options = Seeder.GetOptions(databaseName);

            Seeder.SeedDatabase(options, seed3Clients: true, seed3MoreClients: true);

            using (var assertContext = new PayCloudDbContext(options))
            {
                InitMocks();
                var invalidTerm = "ent100";


                var sut = new ClientService(assertContext, ClientMapperMock.Object, ClientAccountMapperMock.Object);

                //Act
                var actual = await sut.GetClientsCountAsync(invalidTerm);

                //Assert
                Assert.IsTrue(actual == 0);
            }
        }
    }
}

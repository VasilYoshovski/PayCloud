using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Tests.MappersTests
{
    [TestClass]
    public class ClientDtoMapper_MapFrom_Should
    {
        [TestMethod]
        public void MapProperToDto_WhenValidParameterIsPassed()
        {
            //Arrange
            var sut = new ClientDtoMapper();

            var client = new Client
            {
                ClientId = 1,
                Name = "Client1"
            };
            var clientDto = new ClientDto
            {
                ClientId = 1,
                ClientName = "Client1"
                
            };
            //Act
            var actual = sut.MapFrom(client);
            //Assert
            Assert.IsInstanceOfType(actual, typeof(ClientDto));
            Assert.AreEqual(actual.ClientId, client.ClientId);
            Assert.AreEqual(actual.ClientName, client.Name);
        }
    }
}

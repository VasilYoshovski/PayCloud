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
    public class ClientAccountDtoMapper_MapFrom_Should
    {
        [TestMethod]
        public void MapProperToDto_WhenValidParameterIsPassed()
        {
            //Arrange
            var sut = new ClientAccountDtoMapper();

            var account = new Account
            {
                AccountId = 1,
                AccountNumber = "0000000001",
                Balance = 100,
                ClientId = 1
            };
            var accountDto = new ClientAccountDto
            {
                AccountId = 1,
                AccountNumber = "0000000001",
                Balance = 100
            };
            //Act
            var actual = sut.MapFrom(account);
            //Assert
            Assert.IsInstanceOfType(actual, typeof(ClientAccountDto));
            Assert.AreEqual(actual.AccountId, account.AccountId);
            Assert.AreEqual(actual.AccountNumber, account.AccountNumber);
            Assert.AreEqual(actual.Balance, account.Balance);
        }
    }
}

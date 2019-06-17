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
    public class AccountDtoMapper_MapFrom_Should
    {
        [TestMethod]
        public void MapProperToDto_WhenValidParameterIsPassed()
        {
            //Arrange
            var sut = new AccountDtoMapper();

            var account = new Account
            {
                AccountId = 1,
                AccountNumber = "0000000001",
                Balance = 100,
                ClientId = 1,
                Client = new Client {ClientId = 1, Name = "Client1" }
            };
            var accountDto = new AccountDto
            {
                AccountId = 1,
                AccountNumber = "0000000001",
                Balance = 100,
                ClientName = "Client1"
            };
            //Act
            var actual = sut.MapFrom(account);
            //Assert
            Assert.IsInstanceOfType(actual, typeof(AccountDto));
            Assert.AreEqual(actual.AccountId, account.AccountId);
            Assert.AreEqual(actual.AccountNumber, account.AccountNumber);
            Assert.AreEqual(actual.Balance, account.Balance);
            Assert.AreEqual(actual.ClientName, account.Client.Name);
        }
    }
}

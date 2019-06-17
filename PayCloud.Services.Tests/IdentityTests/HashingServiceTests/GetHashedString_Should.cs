using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayCloud.Services.Identity;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Tests.IdentityTests.HashingServiceTests
{
    [TestClass]
    public class GetHashedString_Should
    {
        [TestMethod]
        public void ReturnProperHash_WhenValidINputIsPassed()
        {
            //Arrange
            var sut = new HashingService();
            var validPassword = "12345678";
            var validHash = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F";
            //Act
            var actual = sut.GetHashedString(validPassword);
            //Assert
            Assert.AreEqual(validHash, actual);
        }

        [TestMethod]
        public void ThrowsServiceErrorException_WhenNullInputIsPassed()
        {
            //Arrange
            var sut = new HashingService();
            string invalidPassword = null;
            var validHash = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F";
            //Act && Assert
            Assert.AreEqual(
                   Constants.WrongArguments,
                   Assert.ThrowsException<ServiceErrorException>(() => sut.GetHashedString(invalidPassword)).Message
            );
        }

        [TestMethod]
        public void ThrowsServiceErrorException_WhenEmptyInputIsPassed()
        {
            //Arrange
            var sut = new HashingService();
            string invalidPassword = "";
            var validHash = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F";
            //Act && Assert
            Assert.AreEqual(
                   Constants.WrongArguments,
                   Assert.ThrowsException<ServiceErrorException>(() => sut.GetHashedString(invalidPassword)).Message
            );
        }

    }
}

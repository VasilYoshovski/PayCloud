using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using PayCloud.Data.Models;
using PayCloud.Services.Identity;

namespace PayCloud.Services.Tests.IdentityTests
{
    [TestClass]
    public class TokenManager_Should
    {
        [TestMethod]
        public void ValidateToken_ReturnProperUserName_WhenValidTokenIsPassed()
        {
            var exampleSecret = "d8bec9dfa11de0c600a88b0d5dec0acce9deb0cc812f0865671ca2c2cfa40da8";
            var exampleToken2 = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InN0YWthdGEiLCJ1c2VyUm9sZSI6IkFkbWluIiwibmJmIjoxNTU4MzM2OTcxLCJleHAiOjE1NTg0NDQ5NzAsImlhdCI6MTU1ODMzNjk3MX0.LUz95iTGgKvKqwVaWmRJSG_gnuZ0yS7DPVTkj9FC4Sw";

            var expectedUser = new PayCloudUser()
            {
                UserId = 3,
                Name = "stakata",
                Username = "ExampleUserName3",
                Password = "ExamplePassword3",
                Role = "User",
                UserAccounts = new List<PayCloudUserAccount>(),
                UserClients = new List<PayCloudUserClient>()
            };

            var configuration = new Mock<IConfiguration>();
            var confSection = new Mock<IConfigurationSection>();
            confSection.Setup(x => x.Value).Returns(exampleSecret);

            configuration.Setup(c => c.GetSection("TokenSecrets:JWT"))
                .Returns(confSection.Object);

            var sut = new TokenManager(configuration.Object);

            var result = sut.ValidateToken(exampleToken2);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ValidateToken_ReturnNull_WhenInvalidTokenIsPassed()
        {
            var exampleToken = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var validToken = @"invalid";

            var configuration = new Mock<IConfiguration>();
            var confSection = new Mock<IConfigurationSection>();
            confSection.Setup(x => x.Value).Returns(exampleToken);
            configuration.Setup(c => c.GetSection("TokenSecrets:JWT")).Returns(confSection.Object);

            var sut = new TokenManager(configuration.Object);

            var result = sut.ValidateToken(validToken);

            Assert.AreEqual(false, result);
        }

        //[TestMethod]
        //public void GetPrincipal_Should_ReturnClaimsPrincipal_WhenValidTokenIsPassed()
        //{
        //    var exampleSecret = "d8bec9dfa11de0c600a88b0d5dec0acce9deb0cc812f0865671ca2c2cfa40da8";
        //    var exampleToken2 = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InN0YWthdGEiLCJ1c2VyUm9sZSI6IkFkbWluIiwibmJmIjoxNTU4MzM2OTcxLCJleHAiOjE1NTg0NDQ5NzAsImlhdCI6MTU1ODMzNjk3MX0.LUz95iTGgKvKqwVaWmRJSG_gnuZ0yS7DPVTkj9FC4Sw";
        //    var exampleToken3 = @"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJOYW1lIjoiZXhhbXBsZVVzZXJOYW1lIiwidXNlclJvbGUiOiJVc2VyIiwidXNlcklkIjoiMSIsImp0aSI6ImQ4YmVjOWRmYTExZGUwYzYwMGE4OGIwZDVkZWMwYWNjZTlkZWIwY2M4MTJmMDg2NTY3MWNhMmMyY2ZhNDBkYTgiLCJpYXQiOjE1NTg5NjU1MDYsImV4cCI6MTU1ODk2OTM0Mn0.ECg28w8NuHhglbKxmVFT9fhlK5xRlw7pvPTmYt-J_iY";


        //    var configuration = new Mock<IConfiguration>();
        //    var confSection = new Mock<IConfigurationSection>();
        //    confSection.Setup(x => x.Value).Returns(exampleSecret);
        //    configuration.Setup(c => c.GetSection("TokenSecrets:JWT")).Returns(confSection.Object);

        //    var sut = new TokenManager(configuration.Object);

        //    var result = sut.GetPrincipal(exampleToken3);

        //    Assert.IsInstanceOfType(result, typeof(ClaimsPrincipal));
        //    Assert.IsTrue(result != null);
        //}

        [TestMethod]
        public void GetPrincipal_Should_ReturnNull_WhenInvalidTokenIsPassed()
        {
            var exampleSecret = "d8bec9dfa11de0c600a88b0d5dec0acce9deb0cc812f0865671ca2c2cfa40da8";
            var validToken = @"invalid";

            var configuration = new Mock<IConfiguration>();
            var confSection = new Mock<IConfigurationSection>();
            confSection.Setup(x => x.Value).Returns(exampleSecret);
            configuration.Setup(c => c.GetSection("TokenSecrets:JWT"))
                .Returns(confSection.Object);

            var sut = new TokenManager(configuration.Object);

            var result = sut.GetPrincipal(validToken);

            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void GetToken_Should_ReturnNull_WhenExceptionIsThrown()
        {
            var exampleToken2 = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InN0YWthdGEiLCJ1c2VyUm9sZSI6IkFkbWluIiwibmJmIjoxNTU4MzM2OTcxLCJleHAiOjE1NTg0NDQ5NzAsImlhdCI6MTU1ODMzNjk3MX0.LUz95iTGgKvKqwVaWmRJSG_gnuZ0yS7DPVTkj9FC4Sw";

            var configuration = new Mock<IConfiguration>();
            var confSection = new Mock<IConfigurationSection>();
            confSection.Setup(x => x.Value).Returns("exception throw");
            configuration.Setup(c => c.GetSection("TokenSecrets:JWT"))
                .Returns(confSection.Object);

            var sut = new TokenManager(configuration.Object);

            var result = sut.GetPrincipal(exampleToken2);

            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void GenerateToken_Should_ReturnNotEmptyOrNullString_WhenValidParameterIsPassed()
        {
            var exampleSecret = "d8bec9dfa11de0c600a88b0d5dec0acce9deb0cc812f0865671ca2c2cfa40da8";

            var configuration = new Mock<IConfiguration>();
            var confSection = new Mock<IConfigurationSection>();
            confSection.Setup(x => x.Value).Returns(exampleSecret);
            configuration.Setup(c => c.GetSection("TokenSecrets:JWT"))
                .Returns(confSection.Object);

            var sut = new TokenManager(configuration.Object);

            var result = sut.GenerateToken("valid", "Admin", "1");

            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
    }
}

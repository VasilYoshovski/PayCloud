using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Services.Identity;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PayCloud.Services.Tests.IdentityTests
{
    [TestClass]
    public class AuthorizationService_Should
    {
       
        [TestMethod]
        public void IsInRole_Should_ThrowUnathorizedAccessException_WhenUserRoleIsNotAuthorized()
        {

            var tokenManagerMock = new Mock<ITokenManager>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var exampleToken = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            httpContextAccessor.Setup(x => x.HttpContext.Request.Cookies["SecurityToken"]).Returns(exampleToken);

            tokenManagerMock.Setup(x => x.ValidateToken(exampleToken)).Returns(true);

            tokenManagerMock.Setup(x => x.GetPrincipal(exampleToken)).Returns(TestEntities.exampleClaimPrincipal);

            var sut = new AuthorizationService(tokenManagerMock.Object, httpContextAccessor.Object);

            var actual = sut.IsInRole("Admin");

            Assert.IsFalse(actual);

            //var ex = Assert.ThrowsException<UnauthorizedAccessException>(() =>
            //    sut.IsInRole("Admin"));

            //Assert.AreEqual(ex.Message, Constants.NotAuthorized);
        }

        [TestMethod]
        public void GetUserId_Should_GetCorrectUserId()
        {
            var tokenManagerMock = new Mock<ITokenManager>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var exampleToken = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            httpContextAccessor.Setup(x => x.HttpContext.Request.Cookies["SecurityToken"]).Returns(exampleToken);

            var eaxmpleClaimCollection = new List<Claim>
            {
                new Claim("userRole", "User"),
                new Claim("userId", "1")
            };

            var exampleClaimsIdentity = new ClaimsIdentity(eaxmpleClaimCollection);

            var exampleClaimPrincipal = new ClaimsPrincipal(exampleClaimsIdentity);


            tokenManagerMock.Setup(x => x.GetPrincipal(It.IsAny<string>())).Returns(exampleClaimPrincipal);

            var sut = new AuthorizationService(tokenManagerMock.Object, httpContextAccessor.Object);

            var result = sut.GetLoggedUserId();

            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void GetUserId_Should_ThrowUnathorizedAccessException_WhenNoUserLogged()
        {
            var tokenManagerMock = new Mock<ITokenManager>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var exampleToken = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            httpContextAccessor.Setup(x => x.HttpContext.Request.Cookies["SecurityToken"]).Returns(exampleToken);

            tokenManagerMock.Setup(x => x.ValidateToken(TestEntities.exampleToken)).Returns(value: false);

            var sut = new AuthorizationService(tokenManagerMock.Object, httpContextAccessor.Object);

            var ex = Assert.ThrowsException<UnauthorizedAccessException>(() =>
                sut.GetLoggedUserId());

            Assert.AreEqual(Constants.NotAuthorized, ex.Message);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayCloud.Services.Identity;

namespace PayCloud.Services.Tests.IdentityTests
{
    [TestClass]
    public class CookieManager_Should
    {
        [TestMethod]
        public void AddSessionCookieForToken_ShouldCall_ResponseCookiesAppend()
        {
            var exampleToken = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            var contextMock = new Mock<IHttpContextAccessor>();

            contextMock.Setup(x => x.HttpContext
                                    .Response
                                    .Cookies
                                    .Append("SecurityToken",
                                     exampleToken))
                                    .Verifiable();

            var sut = new CookieManager(contextMock.Object);

            sut.AddSessionCookieForToken(
                exampleToken,
                "username");

            contextMock
                .Verify(x => x.HttpContext
                                .Response
                                .Cookies
                                .Append(
                                "SecurityToken",
                                exampleToken,
                                It.IsAny<CookieOptions>()),
                                        Times.Once);
        }

        [TestMethod]
        public void DeleteSessionCookies_ShouldCall_ResponseCookiesDelete()
        {
            var contextMock = new Mock<IHttpContextAccessor>();

            contextMock.Setup(x => x.HttpContext
                                    .Response
                                    .Cookies
                                    .Delete("SecurityToken"))
                                    .Verifiable();

            var sut = new CookieManager(contextMock.Object);

            sut.DeleteSessionCookies();

            contextMock
                .Verify(x => x.HttpContext
                                .Response
                                .Cookies
                                .Delete("SecurityToken"),
                                        Times.Once);
        }
    }
}

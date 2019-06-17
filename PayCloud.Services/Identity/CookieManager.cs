using Microsoft.AspNetCore.Http;
using PayCloud.Services.Identity.Contracts;
using System;

namespace PayCloud.Services.Identity
{
    public class CookieManager : ICookieManager
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CookieManager(
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void AddSessionCookieForToken(string token, string userName)
        {
            httpContextAccessor
                .HttpContext
                .Response
                .Cookies
                .Append("SecurityToken",
                        token,
                        new CookieOptions
                        {
                            Expires = DateTime.Now.AddMinutes(30)
                        });

            httpContextAccessor
                .HttpContext
                .Response
                .Cookies
                .Append("LoginInfo",
                        userName,
                        new CookieOptions
                        {
                            Expires = DateTime.Now.AddMinutes(30)
                        });
        }

        public void DeleteSessionCookies()
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete("SecurityToken");
            httpContextAccessor.HttpContext.Response.Cookies.Delete("LoginInfo");
        }
    }
}

using Microsoft.AspNetCore.Http;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PayCloud.Services.Identity
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ITokenManager tokenManager;
        private readonly IHttpContextAccessor contextAccessor;

        public AuthorizationService(ITokenManager tokenManager, IHttpContextAccessor contextAccessor)
        {
            this.tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            this.contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public int GetLoggedUserId()
        {
            var userClaimPrinciple = this.tokenManager.GetPrincipal(this.GetSecurityTokenFromCookie());

            if (userClaimPrinciple == null)
            {
                throw new UnauthorizedAccessException(Constants.NotAuthorized);
            }

            var isIdValid = int.TryParse(userClaimPrinciple
                                            .Claims
                                            .Where(c => c.Type == "userId")
                                            .First()
                                            .Value,
                                            out var userId);

            if (isIdValid)
            {
                return userId;
            }
            else
            {
                throw new ServiceErrorException(Constants.UserError);

            }
        }

        //public bool IsAuthorized()
        //{
        //    var userClaimsPrinciple = this.tokenManager.GetPrincipal(this.GetSecurityTokenFromCookie());
        //    if (userClaimsPrinciple == null)
        //    {
        //        throw new UnauthorizedAccessException(Constants.NotAuthorized);
        //    }
        //    return this.tokenManager.ValidateToken(this.GetSecurityTokenFromCookie());
        //}

        private string GetSecurityTokenFromCookie()
        {
            return this.contextAccessor.HttpContext.Request.Cookies["SecurityToken"];
        }

        public bool IsInRole(string role)
        {

            var userClaimsPrinciple = this.tokenManager.GetPrincipal(this.GetSecurityTokenFromCookie());
            if (userClaimsPrinciple == null)
            {
                throw new UnauthorizedAccessException(Constants.NotAuthorized);
            }

            var tokenRole = userClaimsPrinciple.Claims.Where(c => c.Type == "userRole").First().Value;

            return tokenRole == role ? true : false;

        }
    }
}

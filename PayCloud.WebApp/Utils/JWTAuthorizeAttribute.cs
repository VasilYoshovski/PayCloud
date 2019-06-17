using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using PayCloud.Services.Identity.Contracts;
using System;

namespace PayCloud.WebApp.Utils
{
    public class JWTAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string role;

        public JWTAuthorizeAttribute() : base()
        {
        }

        public JWTAuthorizeAttribute(string role) : base()
        {
            this.role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authService = AuthService(context);

           
                if (!authService.IsInRole(role))
                {
                    //vny tozi kod e nuzen, da se razkomentari po-kasno
                    throw new UnauthorizedAccessException(Constants.NotAuthorized);
                }
            
        }

        protected IAuthorizationService AuthService(ActionExecutingContext context)
        {
            return context.HttpContext.RequestServices.GetService<IAuthorizationService>();
        }
    }
}

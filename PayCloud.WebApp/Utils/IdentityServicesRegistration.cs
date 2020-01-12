using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PayCloud.Services.Identity;
using PayCloud.Services.Identity.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Utils
{
    public static class IdentityServiceregistration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdminServices, AdminServices>();
            services.AddSingleton<IHashingService, HashingService>();
            services.AddTransient<ITokenManager, TokenManager>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICookieManager, CookieManager>();
            
            return services;
        }
    }
}

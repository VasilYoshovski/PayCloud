using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PayCloud.Services;
using PayCloud.Services.Contracts;
using PayCloud.Services.Identity;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Providers;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Utils
{
    public static class BusinessServicesRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<IFileServicesProvider, FileServicesProvider>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IBannerServices, BannerServices>();
            services.AddTransient<IRandom10Generator, Random10Generator>();
            services.AddTransient<IDateTimeNowProvider, DateTimeNowProvider>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IPayCloudUserServices, PayCloudUserServices>();
            services.AddTransient<ITransactionServices, TransactionServices>();


            return services;
        }
    }
}

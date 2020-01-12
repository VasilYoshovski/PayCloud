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
            services.AddScoped<IFileServicesProvider, FileServicesProvider>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBannerServices, BannerServices>();
            services.AddSingleton<IRandom10Generator, Random10Generator>();
            services.AddSingleton<IDateTimeNowProvider, DateTimeNowProvider>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IPayCloudUserServices, PayCloudUserServices>();
            services.AddScoped<ITransactionServices, TransactionServices>();
            services.AddSingleton<IRandomProvider, RandomProvider>();

            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using System.Collections.Generic;

namespace PayCloud.Services.Mappers
{
    public static class MapperRegistration
    {
        public static IServiceCollection AddDtoMappers(this IServiceCollection services)
        {
            services.AddSingleton<IDtoMapper<Account, ClientAccountDto>, ClientAccountDtoMapper>();
            services.AddSingleton<IDtoMapper<Account, AccountDto>, AccountDtoMapper>();
            services.AddSingleton<IDtoMapper<Client, ClientDto>, ClientDtoMapper>();
            services.AddSingleton<IDtoMapper<Account, AccountDto>, AccountDtoMapper>();
            
            return services;
        }
    }
}

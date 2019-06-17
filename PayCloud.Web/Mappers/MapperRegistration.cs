using Microsoft.Extensions.DependencyInjection;
using PayCloud.Data.Models;
using PayCloud.Web.Models.ClientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Web.Mappers
{
    public static class MapperRegistration
    {
        public static IServiceCollection AddCustomMappers(this IServiceCollection services)
        {
            services.AddSingleton<IViewModelMapper<Client, ClientViewModel>, ClientViewModelMapper>();
            services.AddSingleton<IViewModelMapper<List<Client>, ClientsCollectionViewModel>, ClientsCollectionViewModelMapper>();

            return services;
        }
    }
}

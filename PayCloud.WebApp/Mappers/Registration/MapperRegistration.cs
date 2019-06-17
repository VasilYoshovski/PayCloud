using Microsoft.Extensions.DependencyInjection;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.WebApp.Areas.Admin.Models.AccountViewModels;
using PayCloud.WebApp.Areas.Admin.Models.BannerViewModels;
using PayCloud.WebApp.Areas.Admin.Models.PayCloudUserViewModels;
using PayCloud.WebApp.Areas.User.Models.AccountViewModels;
using PayCloud.WebApp.Areas.User.Models.TransactionViewModels;

using System.Collections.Generic;

namespace PayCloud.WebApp.Mappers
{
    public static class MapperRegistration
    {
        public static IServiceCollection AddCustomMappers(this IServiceCollection services)
        {
            services.AddSingleton<IViewModelMapper<AccountDto, AccountViewModel>, AccountViewModelMapper>();
            services.AddSingleton<IViewModelMapper<AccountDto, UserAccountViewModel>, UserAccountViewModelMapper>();
            services.AddSingleton<IViewModelMapper<TransactionDto, TransactionViewModel>, TransactionViewModelMapper>();
            services.AddSingleton<IViewModelMapper<TransactionCUViewModel, TransactionCUDto>, TransactionCUDtoMapper>();
            services.AddSingleton<IViewModelMapper<Banner, BannerViewModel>, BannerViewModelMapper>();
            services.AddSingleton<IViewModelMapper<IReadOnlyCollection<Banner>, BannersCollectionViewModel>, BannersCollectionViewModelMapper>();
            services.AddSingleton<IViewModelMapper<PayCloudUser, PayCloudUserViewModel>, PayCloudUserViewModelMapper>();
            services.AddSingleton<IViewModelMapper<IReadOnlyCollection<PayCloudUser>, PayCloudUsersCollectionViewModel>, PayCloudUsersCollectionViewModelMapper>();

            return services;
        }
    }
}

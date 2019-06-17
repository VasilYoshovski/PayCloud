using PayCloud.Data.Models;
using PayCloud.WebApp.Areas.Admin.Models.PayCloudUserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Mappers
{
    public class PayCloudUsersCollectionViewModelMapper : IViewModelMapper<IReadOnlyCollection<PayCloudUser>, PayCloudUsersCollectionViewModel>
    {
        private readonly IViewModelMapper<PayCloudUser, PayCloudUserViewModel> PayCloudUserMapper;

        public PayCloudUsersCollectionViewModelMapper(
            IViewModelMapper<PayCloudUser, PayCloudUserViewModel> payCloudUserMapper)
        {
            this.PayCloudUserMapper = payCloudUserMapper ?? throw new ArgumentNullException(nameof(payCloudUserMapper));
        }

        public PayCloudUsersCollectionViewModel MapFrom(IReadOnlyCollection<PayCloudUser> entity)
             => new PayCloudUsersCollectionViewModel
             {
                 PayCloudUsers = entity.Select(this.PayCloudUserMapper.MapFrom).ToList(),
             };
    }
}

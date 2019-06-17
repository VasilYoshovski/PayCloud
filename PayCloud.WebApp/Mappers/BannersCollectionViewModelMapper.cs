using PayCloud.Data.Models;
using PayCloud.WebApp.Areas.Admin.Models.BannerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Mappers
{
    public class BannersCollectionViewModelMapper : IViewModelMapper<IReadOnlyCollection<Banner>, BannersCollectionViewModel>
    {
        private readonly IViewModelMapper<Banner, BannerViewModel> BannerMapper;

        public BannersCollectionViewModelMapper(
            IViewModelMapper<Banner, BannerViewModel> bannerMapper)
        {
            this.BannerMapper = bannerMapper ?? throw new ArgumentNullException(nameof(bannerMapper));
        }

        public BannersCollectionViewModel MapFrom(IReadOnlyCollection<Banner> entity)
             => new BannersCollectionViewModel
             {
                 Banners = entity.Select(this.BannerMapper.MapFrom).ToList(),
             };
    }
}

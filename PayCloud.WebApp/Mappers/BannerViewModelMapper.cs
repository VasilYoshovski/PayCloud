using PayCloud.Data.Models;
using PayCloud.WebApp.Areas.Admin.Models.BannerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Mappers
{
    public class BannerViewModelMapper : IViewModelMapper<Banner, BannerViewModel>
    {
        public BannerViewModel MapFrom(Banner entity)
        =>
            new BannerViewModel()
            {
                BannerId = entity.BannerId,
                UrlLink = entity.UrlLink,
                ImageLocationPath = entity.ImgLocationPath,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                ImageData = null
            };
    }
}

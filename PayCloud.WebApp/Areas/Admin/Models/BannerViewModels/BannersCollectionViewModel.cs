using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Models.BannerViewModels
{
    public class BannersCollectionViewModel
    {
        public IReadOnlyCollection<BannerViewModel> Banners { get; set; }
        public bool CanEdit { get; set; }
    }
}

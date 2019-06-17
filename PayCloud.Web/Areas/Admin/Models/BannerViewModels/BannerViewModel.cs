using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Web.Areas.Admin.Models.BannerViewModels
{
    public class BannerViewModel
    {
        public string BannerPath { set; get; }
        public string BannerURL { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
    }
}

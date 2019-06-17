using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Models.BannerViewModels
{
    public class BannerViewModel
    {
        [Required]
        public int BannerId { get; set; }

        [Required]
        [MinLength(3)]
        public string UrlLink { get; set; }

        [Required]
        [MinLength(3)]
        public string ImageLocationPath { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public IFormFile ImageData { get; set; }
    }
}

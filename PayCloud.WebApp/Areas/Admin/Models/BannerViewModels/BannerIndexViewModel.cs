using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Models.BannerViewModels
{
    public class BannerIndexViewModel
    {
        [Required]
        public int BannerActivityType { get; set; }

        [Required]
        public BannersCollectionViewModel BannersList { get; set; }

        [Required]
        public int PageIndex { get; set; }

        [Required]
        public bool HasPreviousPage { get; set; }

        [Required]
        public bool HasNextPage { get; set; }

        [Required]
        public int TotalPages { get; set; }

        [Required]
        public int ElementsPerPage { get; set; }

        [Required]
        public int AllDatabaseBannersCount { get; set; }

        [Required]
        public string SearchString { get; set; }

        [Required]
        public int ImageMaxSize { get; set; }

        [Required]
        public BannerViewModel BannerViewModelObject { get; set; }

    }
}

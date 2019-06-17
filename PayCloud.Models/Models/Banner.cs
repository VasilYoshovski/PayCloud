using System;
using System.ComponentModel.DataAnnotations;

namespace PayCloud.Data.Models
{
    public class Banner
    {
        [Required]
        public int BannerId { get; set; }

        [Required]
        [MinLength(3)]
        public string UrlLink { get; set; }

        [Required]
        [MinLength(3)]
        public string ImgLocationPath { get; set; }
        //public string ImageLocationPath { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}

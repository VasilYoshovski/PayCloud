using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Models.AccountViewModels
{
    public class AccountCreateViewModel
    {
        //[Required]
        public int ClientId { get; set; }

        //[Required]
        //[Range(0, double.PositiveInfinity)]
        public decimal Balance { get; set; }

    }
}

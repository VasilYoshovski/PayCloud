using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Models.PayCloudUserViewModels
{
    public class AccountSearchDTO
    {
        public int ClientId { set; get; }
        public int UserId { set; get; }
        public string Term { set; get; }
    }
}

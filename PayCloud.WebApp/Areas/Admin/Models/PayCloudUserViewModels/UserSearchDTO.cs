using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Models.PayCloudUserViewModels
{
    public class UserSearchDTO
    {
        public int? ClientId { set; get; }
        public string Term { set; get; }
    }
}

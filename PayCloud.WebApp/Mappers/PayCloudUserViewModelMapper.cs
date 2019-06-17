using PayCloud.Data.Models;
using PayCloud.WebApp.Areas.Admin.Models.PayCloudUserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Mappers
{
    public class PayCloudUserViewModelMapper : IViewModelMapper<PayCloudUser, PayCloudUserViewModel>
    {
        public PayCloudUserViewModel MapFrom(PayCloudUser entity)
        =>
            new PayCloudUserViewModel()
            {
                UserId = entity.UserId,
                Name = entity.Name,
                Username = entity.Username,
                Password = entity.Password,
                Role = entity.Role
            };
    }
}

using Microsoft.EntityFrameworkCore;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Contracts;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Identity
{
    public class AdminServices : IAdminServices
    {
        private readonly PayCloudDbContext context;
        private readonly IAuthorizationService authorizationService;
        private readonly IHashingService hashingService;

        public AdminServices(PayCloudDbContext context, IAuthorizationService authorizationService, IHashingService hashingService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            this.hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        }
        public async Task<PayCloudAdmin> GetAdminAsync(string username, string password)
        {
            var admin = await context.PayCloudAdmins.AsNoTracking().SingleOrDefaultAsync(x => x.Username == username);

            //admin = new PayCloudAdmin() {
            //    Username = "DemoAdmin",
            //    Password = "a1234567",
            //    Hash = "9626C7444717AAB7A3BBDD509BCAFA35A7491E9478D421B38E539A621F695EDD"
            //    Role = "Admin"
            //};

            if (admin == null)
            {
                throw new ServiceErrorException(Constants.WrongCredentials);
            }

            var hashedPassword = hashingService.GetHashedString(password);

            if (admin.Password != hashedPassword)
            {
                throw new ServiceErrorException(Constants.WrongCredentials);
            }

            admin.Password = "";
            return admin;
        }

        public async Task<PayCloudAdmin> GetLoggedAdminAsync()
        {
            var userId = this.authorizationService.GetLoggedUserId();

            var admin =await context.PayCloudAdmins.FindAsync(userId);

            if (admin == null)
            {
                throw new ServiceErrorException(string.Format(Constants.UserNotFound, userId));
            }

            admin.Password = "";
            return admin;
        }
    }
}

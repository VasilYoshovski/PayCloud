using Microsoft.EntityFrameworkCore;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly PayCloudDbContext context;
        private readonly IAuthorizationService authorizationService;
        private readonly IHashingService hashingService;

        public UserService(PayCloudDbContext context, IAuthorizationService authorizationService, IHashingService hashingService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            this.hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        }


        public async Task<bool> IsUserAuthorizedForAccount(int accountId)
        {
            var ua = await this.context.UsersAccounts.FindAsync(authorizationService.GetLoggedUserId(), accountId);
            return ua==null?false:true; //TODO uncommented
        }

        public async Task<PayCloudUser> GetUserAsync(string username, string password)
        {
            var user = await this.context.PayCloudUsers.SingleOrDefaultAsync(x => x.Username == username);

            //user = new PayCloudUser() {
            //    Username = "DemoUser",
            //    Password = "a1234567",
            //    Hash = "9626C7444717AAB7A3BBDD509BCAFA35A7491E9478D421B38E539A621F695EDD"
            //    Role = "User"
            //};

            if (user == null)
            {
                throw new ServiceErrorException(string.Format(Constants.WrongCredentials));
            }

            var hashedPassword = hashingService.GetHashedString(password);

            if (user.Password != hashedPassword)
            {
                throw new ServiceErrorException(Constants.WrongCredentials);
            }

            user.Password = "";

            return user;
        }

        public async Task<PayCloudUser> GetLoggedUserAsync()
        {
            var userId = this.authorizationService.GetLoggedUserId();

            var user = await context.PayCloudUsers.FindAsync(userId);

            if (user == null)
            {
                throw new ServiceErrorException(string.Format(Constants.UserNotFound,userId));
            }

            user.Password = "";

            return user;
        }
    }
}

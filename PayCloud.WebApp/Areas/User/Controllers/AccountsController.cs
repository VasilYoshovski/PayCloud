using Microsoft.AspNetCore.Mvc;
using PayCloud.Services;
using PayCloud.Services.Dto;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Utils;
using PayCloud.WebApp.Areas.User.Models.AccountViewModels;
using PayCloud.WebApp.Mappers;
using PayCloud.WebApp.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.User.Controllers
{
    [Area("User")]
    [JWTAuthorizeAttribute("User")]
    public class AccountsController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IClientService clientService;
        private readonly IAuthorizationService authorizationService;
        private readonly IViewModelMapper<AccountDto, UserAccountViewModel> accountMapper;

        public AccountsController(
            IAccountService accountService,
            IClientService clientService,
            IAuthorizationService authorizationService,
            IViewModelMapper<AccountDto, UserAccountViewModel> accountMapper)
        {
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            this.clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            this.authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            this.accountMapper = accountMapper ?? throw new ArgumentNullException(nameof(accountMapper));
        }

        // GET: User/Accounts

        public async Task<IActionResult> Index()
        {
            this.ViewData["Title"] = "ACCOUNTS";
            return this.View();
        }

        public async Task<IActionResult> GetAccountsList(
                    string sortOrder,
                    string currentFilter,
                    int? clientId,
                    int? pageNumber,
                    int? pageSize)
        {
            this.ViewData["AccNumSortParm"] = (string.IsNullOrEmpty(sortOrder) || sortOrder == "AccountNumber") ? "AccountNumber_desc" : "AccountNumber";
            this.ViewData["BalanceSortParm"] = sortOrder == "Balance" ? "Balance_desc" : "Balance";
            this.ViewData["NickNameSortParm"] = sortOrder == "NickName" ? "NickName_desc" : "NickName";
            this.ViewData["ClientNameSortParm"] = sortOrder == "ClientName" ? "ClientName_desc" : "ClientName";
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["LengthOptions"] = new int[] { 5, 10, 15, 20 };

            this.ViewData["CurrentFilter"] = currentFilter;
            this.ViewData["ClientId"] = clientId;

            if (pageSize == null)
            {
                pageSize = 5;
            }
            this.ViewData["pageSize"] = pageSize;

            var pageIndex = pageNumber ?? 1;

            var userId = this.authorizationService.GetLoggedUserId();

            var accounts = await this.accountService.GetAllUserAccountsAsync(userId, (pageIndex - 1) * (int)pageSize, (int)pageSize, currentFilter, clientId, sortOrder);
            var accountsCount = await this.accountService.GetUserAcountsCountAsync(userId, currentFilter, clientId);

            var accountViewModelList = accounts.Select(this.accountMapper.MapFrom);
            var result = PaginatedList<UserAccountViewModel>.Create(accountViewModelList, accountsCount, pageIndex, (int)pageSize, true);

            return this.PartialView("_UserAccountsListPartial", result);
        }

        public async Task<IActionResult> GetPieChartInfo()
        {
            var userId = this.authorizationService.GetLoggedUserId();

            var accounts = await this.accountService.GetAccountsForPieAsync(userId);
            return this.Json(accounts);
        }


        public async Task<IActionResult> GetLineChartInfo(int accountId)
        {
            var result = await accountService.LineChartInfo(accountId, 7);
            var formatedResult = result.Select(
                x => new
                {
                    Date = x.Date.ToString(format:"dd.MM"),
                    Balance = x.Balance
                });
            return this.Json(formatedResult);
        }

        // GET: Products list JSON for AJAX
        public async Task<IActionResult> AllAccountsList(string term)
        {
            return this.Json(await this.accountService.GetAllAccountsAsync(0, 5, contains: term));
        }

        // GET: Products list JSON for AJAX
        public async Task<IActionResult> UserAccountsList(string term)
        {
            var userId = this.authorizationService.GetLoggedUserId();

            return this.Json(await this.accountService.GetAllAccountsAsync(0, 5, contains: term, addBalance: true, userId: userId));
        }

        public async Task<IActionResult> AccountDetails(int accountId)
        {
            var accountDto = await accountService.GetAccountByIdAsync(accountId);
            var userId = this.authorizationService.GetLoggedUserId();

            accountDto.NickName = await accountService.GetAccountNicknameAsync(accountId, userId);
            return this.PartialView("_AccountDetailsPartial", accountMapper.MapFrom(accountDto));
        }

        [HttpGet]
        public async Task<IActionResult> ChangeNickname(int accountId)
        {
            var userId = this.authorizationService.GetLoggedUserId();
            var nicknameVieModel = new AccountNickname()
            {
                Nickname = await accountService.GetAccountNicknameAsync(accountId, userId),
                AccountId = accountId
            };

            return this.PartialView("_NicknamePartial",nicknameVieModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeNickname([FromBody]AccountNickname accountNickname)
        {
            try
            {
                var userId = this.authorizationService.GetLoggedUserId();
                await this.accountService.ChangeNicknameAsync(accountNickname.AccountId, userId, accountNickname.Nickname);
                return this.Ok(Json(string.Format(Constants.NicknameChanged,accountNickname.Nickname)));
            }
            catch (ServiceErrorException ex)
            {
                return this.BadRequest(this.Json(ex.Message));
            }
            catch (Exception)
            {
                return this.BadRequest(this.Json(Constants.CommonError));
            }

        }

    }
}

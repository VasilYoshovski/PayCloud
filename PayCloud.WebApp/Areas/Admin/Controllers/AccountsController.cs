using Microsoft.AspNetCore.Mvc;
using PayCloud.Services;
using PayCloud.Services.Dto;
using PayCloud.WebApp.Areas.Admin.Models.AccountViewModels;
using PayCloud.WebApp.Mappers;
using PayCloud.WebApp.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [JWTAuthorizeAttribute("Admin")]

    public class AccountsController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IClientService clientService;
        private readonly IViewModelMapper<AccountDto, AccountViewModel> accountMapper;

        public AccountsController(
            IAccountService accountService,
            IClientService clientService,
            IViewModelMapper<AccountDto, AccountViewModel> accountMapper)
        {
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            this.clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            this.accountMapper = accountMapper ?? throw new ArgumentNullException(nameof(accountMapper));
        }

        // GET: Admin/Accounts
        public async Task<IActionResult> Index()
        {
            this.ViewData["Title"] = "PayCloud users administration";
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
            this.ViewData["ClientNameSortParm"] = sortOrder == "Client.Name" ? "Client.Name_desc" : "Client.Name";
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

            var accounts = await this.accountService.GetAllAccountsAsync((pageIndex - 1) * (int)pageSize, (int)pageSize, true, currentFilter, clientId, sortOrder:sortOrder);
            var accountsCount = await this.accountService.GetAcountsCountAsync(currentFilter, clientId);

            var accountViewModelList = accounts.Select(this.accountMapper.MapFrom);
            var result = PaginatedList<AccountViewModel>.Create(accountViewModelList, accountsCount, pageIndex, (int)pageSize, true);

            return this.PartialView("_AccountsListPartial", result);
        }

        public async Task<IActionResult> GetAccountsOfUserList(
                   string sortOrder,
                   string currentFilter,
                   int? userId,
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
            this.ViewData["UserId"] = userId;

            if (pageSize == null)
            {
                pageSize = 5;
            }
            this.ViewData["pageSize"] = pageSize;

            var pageIndex = pageNumber ?? 1;

            var accounts = await this.accountService.GetAllAccountsAsync((pageIndex - 1) * (int)pageSize, (int)pageSize, true, currentFilter, clientId, userId ?? -1, sortOrder: sortOrder);
            var accountsCount = await this.accountService.GetAcountsOfUserCountAsync(0, int.MaxValue, currentFilter, clientId, userId ?? -1, sortOrder: sortOrder);

            var accountViewModelList = accounts.Select(this.accountMapper.MapFrom);
            var result = PaginatedList<AccountViewModel>.Create(accountViewModelList, accountsCount, pageIndex, (int)pageSize, true);

            return this.PartialView("_AccountsOfUserListPartial", result);
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody]AccountCreateViewModel account)
        {
            
                if (account.ClientId <= 0)
                {
                    throw new ArgumentException(Constants.AccountClientIdNull);
                }
                if (account.Balance <= 0)
                {
                    throw new ArgumentException(Constants.AccountBalanceNull);
                }
                var newAccount = await this.accountService.CreateAccountAsync(account.Balance, account.ClientId);
                return this.Ok(this.Json(string.Format(Constants.AccountCreated, newAccount.AccountNumber)));
            
        }

    }
}

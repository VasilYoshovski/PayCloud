using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PayCloud.Services;
using PayCloud.Services.Dto;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Utils;
using PayCloud.WebApp.Areas.User.Models.AccountViewModels;
using PayCloud.WebApp.Areas.User.Models.TransactionViewModels;
using PayCloud.WebApp.Mappers;
using PayCloud.WebApp.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.User.Controllers
{
    [Area("User")]
    [JWTAuthorizeAttribute("User")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionServices transactionServices;
        private readonly IAuthorizationService authorizationService;
        private readonly IAccountService accountService;
        private readonly IViewModelMapper<TransactionDto, TransactionViewModel> transactionMapper;
        private readonly IViewModelMapper<TransactionCUViewModel, TransactionCUDto> transactionCUMapper;
        private readonly IViewModelMapper<AccountDto, UserAccountViewModel> accountMapper;

        public TransactionsController(
            ITransactionServices transactionServices,
            IAuthorizationService authorizationService,
            IAccountService accountService,
            IViewModelMapper<TransactionDto, TransactionViewModel> transactionMapper,
            IViewModelMapper<TransactionCUViewModel, TransactionCUDto> transactionCUMapper,
            IViewModelMapper<AccountDto, UserAccountViewModel> accountMapper)

        {
            this.transactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
            this.authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            this.transactionMapper = transactionMapper ?? throw new ArgumentNullException(nameof(transactionMapper));
            this.transactionCUMapper = transactionCUMapper ?? throw new ArgumentNullException(nameof(transactionCUMapper));
            this.accountMapper = accountMapper ?? throw new ArgumentNullException(nameof(accountMapper));
        }

        public async Task<IActionResult> Index()
        {
            this.ViewData["Title"] = "TRANSACTIONS";
            return this.View();
        }

        public async Task<IActionResult> ShowPaymentPartial(int? senderAccountId)
        {
            if (senderAccountId == null)
            {
                return this.PartialView("_MakePaymentPartial");
            }

            var account = await this.accountService.GetAccountByIdAsync((int)senderAccountId);

            return this.PartialView("_MakePaymentPartial", this.accountMapper.MapFrom(account));
        }


        public async Task<IActionResult> GetTransactionsList(
                  int? accountId,
                  string sortOrder,
                  string currentFilter,
                  int? pageNumber,
                  int? pageSize)
        {
            this.ViewData["CreatedOnSortParm"] = (string.IsNullOrEmpty(sortOrder) || sortOrder == "CreatedOn") ? "CreatedOn_desc" : "CreatedOn";
            this.ViewData["SentOnSortParm"] = sortOrder == "SentOn" ? "SentOn_desc" : "SentOn";
            this.ViewData["MainAccNumSortParm"] = sortOrder == "MainAccountNum" ? "MainAccountNum_desc" : "MainAccountNum";
            this.ViewData["SecondAccNumSortParm"] = sortOrder == "SecondAccountNum" ? "SecondAccountNum_desc" : "SecondAccountNum";
            this.ViewData["AmountSortParm"] = sortOrder == "Amount" ? "Amount_desc" : "Amount";
            this.ViewData["DescriptionSortParm"] = sortOrder == "Description" ? "Description_desc" : "Description";
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["LengthOptions"] = new int[] { 5, 10, 15, 20 };

            this.ViewData["AccountId"] = accountId;
            this.ViewData["CurrentFilter"] = currentFilter;

            if (pageSize == null)
            {
                pageSize = 5;
            }
            this.ViewData["pageSize"] = pageSize;

            var pageIndex = pageNumber ?? 1;

            var userId = this.authorizationService.GetLoggedUserId();

            var transactions = await this.transactionServices.GetTransactionsListAsync(userId, accountId, (pageIndex - 1) * (int)pageSize, (int)pageSize, currentFilter, sortOrder: sortOrder);
            var transactionsCount = await this.transactionServices.GetTransactionsCountAsync(userId, accountId, currentFilter);

            var transactionViewModelList = transactions.Select(this.transactionMapper.MapFrom);
            var result = PaginatedList<TransactionViewModel>.Create(transactionViewModelList, transactionsCount, pageIndex, (int)pageSize, true);

            return this.PartialView("_AccountTransactionsListPartial", result);
        }

        public async Task<IActionResult> GetMyTransactionsList(
                  int? accountId,
                  string sortOrder,
                  string currentFilter,
                  int? pageNumber,
                  int? pageSize)
        {
            this.ViewData["CreatedOnSortParm"] = (string.IsNullOrEmpty(sortOrder) || sortOrder == "CreatedOn") ? "CreatedOn_desc" : "CreatedOn";
            this.ViewData["SentOnSortParm"] = sortOrder == "SentOn" ? "SentOn_desc" : "SentOn";
            this.ViewData["MainAccNumSortParm"] = sortOrder == "MainAccountNum" ? "MainAccountNum_desc" : "MainAccountNum";
            this.ViewData["SecondAccNumSortParm"] = sortOrder == "SecondAccountNum" ? "SecondAccountNum_desc" : "SecondAccountNum";
            this.ViewData["AmountSortParm"] = sortOrder == "Amount" ? "Amount_desc" : "Amount";
            this.ViewData["DescriptionSortParm"] = sortOrder == "Description" ? "Description_desc" : "Description";
            this.ViewData["StatusCodeSortParm"] = sortOrder == "StatusCode" ? "StatusCode_desc" : "StatusCode";
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["LengthOptions"] = new int[] { 5, 10, 15, 20 };

            this.ViewData["AccountId"] = accountId;
            this.ViewData["CurrentFilter"] = currentFilter;

            if (pageSize == null)
            {
                pageSize = 5;
            }
            this.ViewData["pageSize"] = pageSize;

            var pageIndex = pageNumber ?? 1;

            var userId = this.authorizationService.GetLoggedUserId();

            var transactions = await this.transactionServices.GetUserTransactionsAsync(userId, accountId, (pageIndex - 1) * (int)pageSize, (int)pageSize, currentFilter, sortOrder: sortOrder);
            var transactionsCount = await this.transactionServices.GetUserTransactionsCountAsync(userId, accountId, currentFilter);

            var transactionViewModelList = transactions.Select(this.transactionMapper.MapFrom);
            var result = PaginatedList<TransactionViewModel>.Create(transactionViewModelList, transactionsCount, pageIndex, (int)pageSize, true);

            return this.PartialView("_MyTransactionsListPartial", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakePayment([FromBody]TransactionCUViewModel payment)
        {
            //try
            //{
                var userId = this.authorizationService.GetLoggedUserId();

                var transactionCUDto = this.transactionCUMapper.MapFrom(payment);

                transactionCUDto.CreatedByUserId = userId;
                transactionCUDto.StatusCode = (int)Data.Models.StatusCode.Saved;
                await this.transactionServices.MakePaymentAsync(transactionCUDto);
                return this.Ok(this.Json(Constants.TransactionSent));
            //}
            //catch (ServiceErrorException seex)
            //{
            //    return this.BadRequest(this.Json(seex.Message));
            //}
            //catch (UnauthorizedAccessException uaex)
            //{
            //    return this.BadRequest(this.Json(uaex.Message));
            //}
            //catch (Exception)
            //{
            //    return this.BadRequest(this.Json(Constants.CommonError));
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePayment([FromBody]TransactionCUViewModel payment)
        {
            //try
            //{
                var userId = this.authorizationService.GetLoggedUserId();

                var transactionCUDto = this.transactionCUMapper.MapFrom(payment);

                transactionCUDto.CreatedByUserId = userId;
                transactionCUDto.StatusCode = (int)Data.Models.StatusCode.Saved;

                await this.transactionServices.SavePaymentAsync(transactionCUDto);

                return this.Ok(this.Json(Constants.TransactionSaved));
            //}
            //catch (ServiceErrorException seex)
            //{
            //    return this.BadRequest(this.Json(seex.Message));
            //}
            //catch (UnauthorizedAccessException uaex)
            //{
            //    return this.BadRequest(this.Json(uaex.Message));
            //}
            //catch (Exception)
            //{
            //    return this.BadRequest(this.Json(Constants.CommonError));
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelSavedPayment([FromBody]int transactionId)
        {
            //try
            //{
                await this.transactionServices.CancelPaymentAsync(transactionId);

                return this.Ok(this.Json(Constants.TransactionSaved));
            //}
            //catch (ServiceErrorException seex)
            //{
            //    return this.BadRequest(this.Json(seex.Message));
            //}
            //catch (UnauthorizedAccessException uaex)
            //{
            //    return this.BadRequest(this.Json(uaex.Message));
            //}
            //catch (Exception)
            //{
            //    return this.BadRequest(this.Json(Constants.CommonError));
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaySavedPayment([FromBody]int transactionId)
        {
            //try
            //{
                await this.transactionServices.SendPayment(transactionId);

                return this.Ok(Json(Constants.TransactionSaved));
            //}
            //catch (ServiceErrorException seex)
            //{
            //    return this.BadRequest(seex.Message);
            //}
            //catch (UnauthorizedAccessException uaex)
            //{
            //    return this.BadRequest(uaex.Message);
            //}
            //catch (Exception)
            //{
            //    return this.BadRequest(Constants.CommonError);
            //}
        }
    }
}

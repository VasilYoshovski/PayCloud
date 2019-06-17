using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services;
using PayCloud.Services.Contracts;
using PayCloud.Services.Dto;
using PayCloud.WebApp.Areas.Admin.Models.AccountViewModels;
using PayCloud.WebApp.Areas.Admin.Models.PayCloudUserViewModels;
using PayCloud.WebApp.Mappers;
using PayCloud.WebApp.Utils;

namespace PayCloud.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [JWTAuthorizeAttribute("Admin")]

    public class PayCloudUsersController : Controller
    {
        private readonly IPayCloudUserServices userServices;
        private readonly IAccountService accountService;
        private readonly IClientService clientService;

        private readonly ILogger<PayCloudUsersController> logger;

        public PayCloudUsersController(
            IPayCloudUserServices userServices,
            IAccountService accountService,
            IClientService clientService,
            ILogger<PayCloudUsersController> logger)
        {
            this.userServices = userServices ?? throw new ArgumentNullException(nameof(userServices));
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            this.clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //public IActionResult LoadCreateUserModal()
        //{
        //    return this.PartialView("_CreateUserPartial");
        //}

        //public async Task<IActionResult> GetPayCloudUsersList(
        //            string sortOrder,
        //            string currentFilter,
        //            int? clientId,
        //            int? userId,
        //            int? pageNumber,
        //            int? pageSize)
        //{
        //    this.ViewData["AccNumSortParm"] = (string.IsNullOrEmpty(sortOrder) || sortOrder == "AccountNumber") ? "AccountNumber_desc" : "AccountNumber";
        //    this.ViewData["BalanceSortParm"] = sortOrder == "Balance" ? "Balance_desc" : "Balance";
        //    this.ViewData["NickNameSortParm"] = sortOrder == "NickName" ? "NickName_desc" : "NickName";
        //    this.ViewData["ClientNameSortParm"] = sortOrder == "ClientName" ? "ClientName_desc" : "ClientName";
        //    this.ViewData["CurrentSort"] = sortOrder;
        //    this.ViewData["LengthOptions"] = new int[] { 5, 10, 15, 20 };

        //    this.ViewData["CurrentFilter"] = currentFilter;
        //    this.ViewData["ClientId"] = clientId;

        //    if (pageSize == null)
        //    {
        //        pageSize = 5;
        //    }
        //    this.ViewData["pageSize"] = pageSize;

        //    var pageIndex = pageNumber ?? 1;
        //    //var accounts = await this.accountService.GetAllAccountsAsync((pageIndex - 1) * (int)pageSize, (int)pageSize, currentFilter, clientId, sortOrder: sortOrder);
        //    var accounts = await this.accountService.GetAllAccountsAsync(0, int.MaxValue, false, currentFilter, clientId, userId, sortOrder: sortOrder);
        //    //TODO vny : да обобщя всички сметки за даден юзер в една обща
        //    var usersList = await this.userServices.GetAllPayCloudUsersAsync();
        //    List<UserModalViewModel> modalUserList = new List<UserModalViewModel>();
        //    foreach (var userItem in usersList)
        //    {
        //        var queryAccountsOfUser = await this.accountService.AccountsIdsAssignedToPayCloudUserAsync(userItem.UserId);
        //        var ballanceOfUser = 0;// accounts.Where(ac => ac..ClientId == clientId).Where(a => queryAccountsOfUser.Any(au => au == a.AccountId)).Select(h => h.Balance).Sum();
        //        UserModalViewModel tmpModalUser = new UserModalViewModel
        //        {
        //            Balance = ballanceOfUser,
        //            //ClientName = ,
        //            NickName = userItem.Username,
        //            UserId = userItem.UserId,
        //            UserName = userItem.Name
        //        };
        //        modalUserList.Add(tmpModalUser);
        //    }
        //    //var accountsCount = await this.accountService.GetAcountsCountAsync(0, int.MaxValue, currentFilter, clientId, sortOrder: sortOrder);
        //    var accountsCount = modalUserList.Count;

        //    var result = PaginatedList<UserModalViewModel>.Create(modalUserList, accountsCount, pageIndex, (int)pageSize, true);

        //    return this.PartialView("_UsersListPartial", result);
        //}
        
        // GET: Products list JSON for AJAX
        public async Task<IActionResult> UsersAssignedToClient([FromBody] UserSearchDTO userDTO)
        {
            var usersList = await this.userServices.PayCloudUsersAssignedToClientAsync(userDTO.ClientId);
            if (!string.IsNullOrWhiteSpace(userDTO.Term))
            {
                userDTO.Term = userDTO.Term.Trim().ToLower();
                usersList = usersList.Where(u => u.Name.ToLower().Contains(userDTO.Term)).ToList();
            }
            var usersInfoList = usersList.Select(u => new UserInfoDTO()
            {
                UserId = u.UserId,
                UserName = u.Name
            }).ToList();
            return this.Json(usersInfoList);
        }

        // GET: Products list JSON for AJAX
        public async Task<IActionResult> UsersNotAssignedToClient([FromBody] UserSearchDTO userDTO)
        {
            var usersList = await this.userServices.PayCloudUsersNotAssignedToClientAsync(userDTO.ClientId);
            if (!string.IsNullOrWhiteSpace(userDTO.Term))
            {
                userDTO.Term = userDTO.Term.Trim().ToLower();
                usersList = usersList.Where(u => u.Name.ToLower().Contains(userDTO.Term)).ToList();
            }
            var usersInfoList = usersList.Select(u => new UserInfoDTO() {
                UserId = u.UserId,
                UserName = u.Name}).ToList();
            return this.Json(usersInfoList);
        }

        // GET: Products list JSON for AJAX
        public async Task<IActionResult> AccountsNotAssignedToUserOfClient([FromBody] AccountSearchDTO accountSearchDTO)
        {
            var accountsIdsOfUser = await this.accountService.AccountsIdsAssignedToPayCloudUserAsync(accountSearchDTO.UserId);
            List<ClientAccountDto> accountsOfClient = (await this.clientService.GetClientAccounts(accountSearchDTO.ClientId)).ToList();
            if (null != accountsIdsOfUser && accountsIdsOfUser.Count > 0)
            {
                accountsOfClient = accountsOfClient.Where(u => accountsIdsOfUser.All(au => au != u.AccountId)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(accountSearchDTO.Term))
            {
                accountSearchDTO.Term = accountSearchDTO.Term.Trim().ToLower();
                accountsOfClient = accountsOfClient
                    .Where(ac => ac.AccountNumber.ToLower().Contains(accountSearchDTO.Term))
                    .ToList();
            }
            var accountsInfoList = accountsOfClient
                .Select(a => new AccountRequestDTO()
                {
                    AccountId = a.AccountId,
                    AccountNumber = a.AccountNumber
                })
                .ToList();
            return this.Json(accountsInfoList);
        }

        // GET: Products list JSON for AJAX
        public async Task<IActionResult> AccountsAssignedToUserOfClient([FromBody] AccountSearchDTO accountSearchDTO)
        {
            var accountsIdsOfUser = await this.accountService.AccountsIdsAssignedToPayCloudUserAsync(accountSearchDTO.UserId);
            if (null == accountsIdsOfUser || accountsIdsOfUser.Count < 1)
            {
                return this.Json(new List<AccountRequestDTO>());
            }
            var accountsOfClient = (await this.clientService.GetClientAccounts(accountSearchDTO.ClientId))
                .Where(u => accountsIdsOfUser.Any(au => au == u.AccountId))
                .ToList();

            if (!string.IsNullOrWhiteSpace(accountSearchDTO.Term))
            {
                accountSearchDTO.Term = accountSearchDTO.Term.Trim().ToLower();
                accountsOfClient = accountsOfClient
                    .Where(ac => ac.AccountNumber.ToLower().Contains(accountSearchDTO.Term))
                    .ToList();
            }
            var accountsInfoList = accountsOfClient
                .Select(a => new AccountRequestDTO()
                {
                    AccountId = a.AccountId,
                    AccountNumber = a.AccountNumber
                })
                .ToList();
            return this.Json(accountsInfoList);
        }

        //// GET: Admin/PayCloudUsers
        //public async Task<IActionResult> Index()
        //{
        //    return View(await this.userServices.GetAllPayCloudUsersAsync());
        //}

        //// GET: Admin/PayCloudUsers/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var payCloudUser = await this.userServices.FindPayCloudUserByIDAsync(id ?? -1);
        //    if (payCloudUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(payCloudUser);
        //}

        //// GET: Admin/PayCloudUsers/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Admin/PayCloudUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] PayCloudUserViewModel payCloudViewUser)
        {
            var role = "User";
            var payCloudUser = new PayCloudUser()
            {
                Name = payCloudViewUser.Name,
                Username = payCloudViewUser.Username,
                Password = payCloudViewUser.Password,
                Role = payCloudViewUser.Role
            };

            if (ModelState.IsValid)
            {
                try
                {
                    await this.userServices.CreatePayCloudUserAsync(
                        payCloudUser.Name,
                        payCloudUser.Username,
                        payCloudUser.Password,
                        role);
                }
                catch (Exception ex)
                {
                    return this.BadRequest(this.Json(ex.Message));
                }
                var createdUser = await this.userServices.FindPayCloudUserByUserNameAsync(payCloudUser.Username);
                if (null == createdUser)
                {
                    return this.BadRequest(this.Json("Error: User not created"));
                }
                return this.Ok(this.Json($"Created user with username {createdUser.Username} and name {createdUser.Name}"));
            }
            return this.BadRequest(this.Json("User input data is invalid"));
        }

        // POST: Admin/PayCloudUsers/AddUserToClient
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToClient([FromBody] ClientUserIdsDTO clientUserIds)
        {
            if (ModelState.IsValid)
            {
                if (null == clientUserIds.ClientId)
                {
                    return this.BadRequest(this.Json($"Error: Unknown client."));
                }
                var user = (await this.userServices.PayCloudUsersAssignedToClientAsync(clientUserIds.ClientId))
                    .Where(u => u.UserId == clientUserIds.UserId)
                    .FirstOrDefault();
                if (null != user)
                {
                    return this.BadRequest(this.Json($"Error: The user is already related to the client."));
                }
                try
                {
                    await this.userServices.AssignClientToUserAsync(
                        clientUserIds.ClientId.Value,
                        clientUserIds.UserId);
                }
                catch (Exception)
                {
                    return this.BadRequest(this.Json($"Error: User not assigned to the client"));
                }
                return this.Ok(this.Json($"User is assigned to the client"));
            }
            return this.BadRequest(this.Json("Input data is invalid"));
        }

        // POST: Admin/PayCloudUsers/RemoveUserFromClient
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserFromClient([FromBody] ClientUserIdsDTO clientUserIds)
        {
            if (ModelState.IsValid)
            {
                if (null == clientUserIds.ClientId)
                {
                    return this.BadRequest(this.Json($"Error: Unknown client."));
                }
                var user = (await this.userServices.PayCloudUsersAssignedToClientAsync(clientUserIds.ClientId))
                    .Where(u => u.UserId == clientUserIds.UserId)
                    .FirstOrDefault();
                if (null == user)
                {
                    return this.BadRequest(this.Json($"Error: The user is nor related to the client."));
                }
                try
                {
                    var deleteResultTemp = await this.userServices.RemoveUserFromClientAsync(
                        clientUserIds.ClientId.Value,
                        clientUserIds.UserId);
                    if (deleteResultTemp)
                    {
                        bool accountsDeleteResult = await this.userServices.DeleteAllAccountsOfUser(clientUserIds.ClientId.Value, clientUserIds.UserId);
                        if (accountsDeleteResult)
                        {
                            throw new Exception($"Error: User with name: {user.Name} is deleted, but it's accounts are not deleted!");
                        }
                    }
                }
                catch (Exception)
                {
                    return this.BadRequest($"Error: User with name: {user.Name} is nor removed from the client.");
                }
                return this.Ok(this.Json($"User with name: {user.Name} is removed from the client"));
            }
            return this.BadRequest(this.Json("Input data is invalid"));
        }

        // POST: Admin/PayCloudUsers/AddAccountToUser
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAccountToUser([FromBody] ClientUserAccountIdsDTO clientUserAccountIds)
        {
            if (ModelState.IsValid)
            {
                if (null == clientUserAccountIds.ClientId)
                {
                    return this.BadRequest(this.Json($"Error: Unknown client."));
                }
                var clientAccount = (await this.clientService.GetClientAccounts(clientUserAccountIds.ClientId.Value))
                    .Where(ca => ca.AccountId == clientUserAccountIds.AccountId)
                    .FirstOrDefault();
                if (null == clientAccount)
                {
                    return this.BadRequest(this.Json($"Error: The account is not related to the client."));
                }
                var user = (await this.userServices.PayCloudUsersAssignedToClientAsync(clientUserAccountIds.ClientId))
                    .Where(u => u.UserId == clientUserAccountIds.UserId)
                    .FirstOrDefault();
                if (null == user)
                {
                    return this.BadRequest(this.Json($"Error: The user is not related to the client."));
                }
                try
                {
                    await this.userServices.AssignAcountToUserAsync(
                        clientUserAccountIds.ClientId.Value,
                        clientUserAccountIds.AccountId,
                        clientUserAccountIds.UserId);
                }
                catch (Exception)
                {
                    return this.BadRequest(this.Json("Error: Invalid Client or User or Account! Check the Account first!"));
                }
                return this.Ok(this.Json($"Account with accountNumber {clientAccount.AccountNumber} is assigned to user with name {user.Name}"));
            }
            return this.BadRequest(this.Json("Input data is invalid"));
        }

        // POST: Admin/PayCloudUsers/RemoveAccountFromUser
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAccountFromUser([FromBody] ClientUserAccountIdsDTO clientUserAccountIds)
        {
            if (ModelState.IsValid)
            {
                if (null == clientUserAccountIds.ClientId)
                {
                    return this.BadRequest(this.Json($"Error: Unknown client."));
                }
                var clientAccount = (await this.clientService.GetClientAccounts(clientUserAccountIds.ClientId.Value))
                    .Where(ca => ca.AccountId == clientUserAccountIds.AccountId)
                    .FirstOrDefault();
                if (null == clientAccount)
                {
                    return this.BadRequest(this.Json($"Error: The account is not related to the client."));
                }
                var user = (await this.userServices.PayCloudUsersAssignedToClientAsync(clientUserAccountIds.ClientId))
                    .Where(u => u.UserId == clientUserAccountIds.UserId)
                    .FirstOrDefault();
                if (null == user)
                {
                    return this.BadRequest(this.Json($"Error: The user is not related to the client."));
                }
                try
                {
                    await this.userServices.RemoveAcountFromUserAsync(
                        clientUserAccountIds.ClientId.Value,
                        clientUserAccountIds.AccountId,
                        clientUserAccountIds.UserId);
                }
                catch (Exception)
                {
                    return this.BadRequest(this.Json("Error: Invalid Client or User or Account! Check the Account first!"));
                }
                return this.Ok(this.Json($"Account is assigned to user with name {user.UserId}"));
            }
            return this.BadRequest(this.Json("Input data is invalid"));
        }

        //// GET: Admin/PayCloudUsers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var payCloudUser = await this.userServices.FindPayCloudUserByIDAsync(id ?? -1);
        //    if (payCloudUser == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(payCloudUser);
        //}

        //// POST: Admin/PayCloudUsers/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Username,Password,Role")] PayCloudUser payCloudUser)
        //{
        //    if (id != payCloudUser.UserId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await this.userServices.UpdatePayCloudUserAsync(
        //                payCloudUser.UserId,
        //                payCloudUser.Name,
        //                payCloudUser.Username,
        //                payCloudUser.Password,
        //                payCloudUser.Role);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (null == this.userServices.FindPayCloudUserByIDAsync(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        //return RedirectToAction(nameof(Index));
        //    }
        //    return View(payCloudUser);
        //}

        //// GET: Admin/PayCloudUsers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var payCloudUser = await this.userServices.FindPayCloudUserByIDAsync(id ?? -1);
        //    if (payCloudUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(payCloudUser);
        //}

        //// POST: Admin/PayCloudUsers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    await this.userServices.DeletePayCloudUserAsync(id);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}

using Microsoft.AspNetCore.Mvc;
using PayCloud.Services;
using PayCloud.Services.Dto;
using PayCloud.WebApp.Utils;
using System;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [JWTAuthorizeAttribute("Admin")]

    public class ClientsController : Controller
    {
        private readonly IClientService clientService;

        // GET: Admin/Clients
        public async Task<IActionResult> Index(bool clientSelect=true)
        {
            this.ViewData["Title"] = "Clients management page";
            return this.View(clientSelect);
        }

        public ClientsController(IClientService clientService)
        {
            this.clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        // GET: Products list JSON for AJAX
        public async Task<IActionResult> ClientList(string term)
        {
            return this.Json(await this.clientService.GetClientsListAsync(0, 10, term:term));
        }

        // POST: Admin/Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody]string clientName)
        {
            try
            {
                var newClient = await this.clientService.CreateClientAsync(clientName);
                return this.Json(string.Format(Constants.ClientCreated, clientName));
            }
            catch (Exception ex)
            {
                return this.BadRequest(this.Json(ex.Message));
            }
        }

        public async Task<IActionResult> GetClientsList(
                   string sortOrder,
                   string currentFilter,
                   int? pageNumber,
                   int? pageSize)
        {
            this.ViewData["ClientIdSortParm"] = (string.IsNullOrEmpty(sortOrder) || sortOrder=="ClientId") ? "ClientId_desc" : "ClientId";
            this.ViewData["ClientNameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["LengthOptions"] = new int[] { 5, 10, 15, 20 };

            this.ViewData["CurrentFilter"] = currentFilter;

            if (pageSize == null)
            {
                pageSize = 5;
            }
            this.ViewData["pageSize"] = pageSize;

            var pageIndex = pageNumber ?? 1;

            var clients = await this.clientService.GetClientsListAsync((pageIndex - 1) * (int)pageSize, (int)pageSize, currentFilter, sortOrder: sortOrder);
            var clientsCount = await this.clientService.GetClientsCountAsync(currentFilter);

            var result = PaginatedList<ClientDto>.Create(clients, clientsCount, pageIndex, (int)pageSize, true);

            return this.PartialView("_ClientsListPartial", result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PayCloud.Services.Identity.Contracts;
using PayCloud.WebApp.Areas.Admin.Models.LoginViewModels;
using PayCloud.WebApp.Models;
using PayCloud.WebApp.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class LoginController : Controller
    {
        private readonly IAuthorizationService authorizationService;

        public LoginController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        public IActionResult AdminLogin(string errorMessage)
        {
            if (errorMessage != null)
            {
                this.ViewData["Error"] = errorMessage;
            }
            return this.View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}

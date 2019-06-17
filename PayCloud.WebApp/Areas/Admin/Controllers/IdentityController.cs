using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayCloud.Services.Identity.Contracts;
using PayCloud.WebApp.Areas.Admin.Models.LoginViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Controllers
{
    public class IdentityController : Controller
    {
        private readonly ITokenManager tokenManager;
        private readonly IAdminServices adminServices;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICookieManager cookieManager;

        public IdentityController(
            IAdminServices adminServices,
            ITokenManager tokenManager,
            IHttpContextAccessor httpContextAccessor,
            ICookieManager cookieManager)
        {
            this.adminServices = adminServices;
            this.tokenManager = tokenManager;
            this.httpContextAccessor = httpContextAccessor;
            this.cookieManager = cookieManager;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AdminLoginViewModel adminModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(adminModel);
                }

                var admin = await this.adminServices.GetAdminAsync(adminModel.Username, adminModel.Password);

                var token = this.tokenManager.GenerateToken(admin.Username, admin.Role, admin.AdminId.ToString());

                this.cookieManager.AddSessionCookieForToken(token, admin.Username);

                if (admin.Role == "Admin")
                {
                    return this.RedirectToAction("Index", "Accounts", new { area = "Admin" });
                }
                else
                {
                    //return RedirectToAction("Index", "Accounts", new { area = "Admin" });

                    return this.RedirectToAction("AdminLogin", "Login", new { area = "Admin" });
                }
            }
            catch (Exception ex)
            {
                return this.RedirectToAction("AdminLogin", "Login",
                    new
                    {
                        area = "Admin",
                        errorMessage = ex.Message
                    });

                //return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            this.cookieManager.DeleteSessionCookies();

            return this.RedirectToAction("AdminLogin", "Login", new { area = "Admin" });
        }

    }
}

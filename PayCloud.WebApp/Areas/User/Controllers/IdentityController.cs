using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayCloud.Services.Identity.Contracts;
using PayCloud.WebApp.Models;
using System;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.User.Controllers
{
    [Area("User")]
    public class IdentityController : Controller
    {
        private readonly ITokenManager tokenManager;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICookieManager cookieManager;

        public IdentityController(
            IUserService userService,
            ITokenManager tokenManager,
            IHttpContextAccessor httpContextAccessor,
            ICookieManager cookieManager)
        {
            this.userService = userService;
            this.tokenManager = tokenManager;
            this.httpContextAccessor = httpContextAccessor;
            this.cookieManager = cookieManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel userModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(userModel);
                }

                var user = await userService.GetUserAsync(userModel.Username, userModel.Password);

                var token = tokenManager.GenerateToken(user.Username, user.Role, user.UserId.ToString());

                cookieManager.AddSessionCookieForToken(token, user.Username);

                if (user.Role == "User")
                {
                    return RedirectToAction("Index", "Accounts", new { area = "User" });
                }
                else
                {
                    return RedirectToAction("AdminLogin");

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home", new { errorMessage = ex.Message });

                //return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                cookieManager.DeleteSessionCookies();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

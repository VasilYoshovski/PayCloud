using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PayCloud.Services.Contracts;
using PayCloud.WebApp.Models;
using PayCloud.WebApp.Utils;

namespace PayCloud.WebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly IBannerServices bannerServices;

        public HomeController(IBannerServices bannerServices)
        {
            this.bannerServices = bannerServices ?? throw new ArgumentNullException(nameof(bannerServices));
        }

        //[ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> Index(string errorMessage)
        {


            LoginViewModel userModel = new LoginViewModel
            {
                Password = "",
                Username = "",
                Banners = await this.bannerServices.GetRandomSublistOfActiveBannersAsync(3)
            };

            if (errorMessage != null)
            {
                this.ViewData["Error"] = errorMessage;
            }

            return View(userModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string errorMessage)
        {
            return View("ErrorPage",errorMessage);
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return RedirectToAction("Error",new { errorMessage = "404! Page not found!"});
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            // handle different codes or just return the default error view
            return RedirectToAction("Error", new { errorMessage = Constants.CommonError });
        }
    }
}
